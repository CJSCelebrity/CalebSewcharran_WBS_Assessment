# Booking System: Design Notes

The brief asks for thinking rather than volume of code, so this document is the
main artefact. It records what I assumed, what I decided, and what I know is
missing.

## Assumptions

From the brief: availability is always guaranteed, so there is no overlap or
double-booking logic. Nothing persists beyond the process.

Where the brief was silent, I assumed:

- Items and users are a seeded, read-only catalogue. The system books items; it
  does not manage them.
- A booking references exactly one item.
- Dates are captured to day precision, parsed with a fixed `yyyy-MM-dd` format.
- Entities generate their own identifiers.
- The console needs an interactive terminal and exits when input closes.

## Items and bookings are separate things

An apartment is not a booking, it is the thing that gets booked. My first model
put a reserved date on the item, which fails immediately: if two people book the
same apartment for different weekends, the second overwrites the first.

So `Apartment`, `Vehicle` and `Show` implement `IBookingInformation` and carry only
identity and a name. `Booking` carries who booked, what they booked, when, and
its lifecycle state. One item now supports many bookings.

## Booking types

`Booking` is an abstract base holding `Id`, `UserId`, `ItemId`, `Status` and
`CancelledAt`. Three subclasses carry their own time shape, because the domains
genuinely differ:

| Type | Time shape |
|---|---|
| `ApartmentBooking` | `CheckIn` / `CheckOut` |
| `VehicleBooking` | `Pickup` / `Dropoff` |
| `ShowBooking` | `PerformanceTime` |

The alternative was one flat `Booking` with `Start`, `End` and a discriminator,
where a show sets `End` equal to `Start`. I rejected it because it creates
fields that do not apply to every type and loses the domain vocabulary. The cost
of my choice is downcasting on retrieval, noted below.

`BookingStatus` is `Reserved` or `Cancelled`. There is no delete anywhere.
Cancelling sets the status and records `CancelledAt`. Cancelled bookings still
appear in queries and cannot be amended or cancelled again.

A booking is evidence that something was agreed, and deleting it destroys the
audit trail. This also matters for the availability question below: overlap
checks would need to exclude cancelled bookings, which is only possible if they
still exist.

## Application layer

Three creation services, one per booking type, because creation and rescheduling
need the subtype's dates. One `BookingManagementService` for cancellation and
retrieval, which work on the base class. Type-specific creation, polymorphic
everything else.

A generic `Create<TBooking>` cannot work: constrained to `Booking`, it cannot
reach `CheckIn` or `Pickup`. The difference would reappear as a factory or a
type switch, which is more code than three typed methods. Generics are used
where the shape really is shared, in `IRepository<T>` for catalogue lookups.

A `BookingService` base class with three subclasses would be a parallel
inheritance hierarchy, forcing a service subclass for every new booking type.
The shared existence checks live in `BookingGuard` instead, composed into all
three services.

CQRS with a mediator is where I would go at roughly eight or more commands. At
three it is ceremony.

## If extending is required in future

The three creation services differ only in their request type, their catalogue
and their constructor call. That repetition is tolerable at three booking types
and becomes the obvious thing to remove at more.

The route out is a factory: an `IBookingFactory` per type, each knowing how to
turn its own request into its own entity, resolved from a registry keyed by
booking type. Creation then collapses to one service that validates, checks
existence, asks the registry for the right factory, and stores the result.
Adding a booking type becomes a new entity, a new request, a new validator and
a new factory registration, with no existing class modified.

I did not do that here. At three types the registry and the lookup are more
code than the duplication they remove, and the indirection makes the creation
path harder to read for someone seeing it for the first time. The repetition is
visible and honest; the factory would be the right trade once the list of
bookable types is genuinely open-ended, which is also the point at which the
mediator route above becomes worth its ceremony.

Errors are exceptions rather than a `Result<T>`, because the entities already
throw and two error models would be worse than one. Each front end translates:
the console prints, the API maps to status codes. At larger scale I would use
results for expected failures and keep exceptions for the exceptional.

## Known limitations

- Retrieval returns `Booking`, so the per-type services downcast. A relational
  store solves this with table-per-hierarchy and a discriminator.
- A wrong-type identifier reports "not found" rather than "wrong type".
- No repository `Update`. In memory, mutating the entity is the update. A real
  database would need an explicit update or a unit of work.
- Shows are booked to day precision and realistically need a time.
- Domain models are returned directly, so the domain shape is the wire
  contract. DTOs would decouple them.
- `DateTime.UtcNow` is called inline. An injected clock would make time
  testable.

**If availability were not guaranteed**, bookings would need overlap detection
against the item, which means exposing a comparable date range rather than
domain-specific fields. That introduces a race between checking and writing:
in memory a lock per item, in a database a transaction with suitable isolation
or a uniqueness constraint that lets the write fail and retry.

## Verification

Manual paths run against the console:

- Book, view, amend, cancel, view: the cancelled booking remains listed with its
  status.
- Amending or cancelling a cancelled booking is rejected, by the entity rather
  than the service.
- Check-out before or equal to check-in is rejected with a readable message.
- Unparseable and locale-ambiguous dates are reprompted, never accepted.
- The same apartment booked twice for different ranges: both persist. This is
  the test that validates the item/booking split.
- All three booking types listed together for one user, each with its own date
  shape.

## Structure

```
Core            no dependencies
Application     depends on Core
Infrastructure  depends on Core/Application
Api             depends on Core/Application
Console         depends on Core/Application
```

`Infrastructure`, `Api` and `Console` are siblings and none references another.
That is the point of building two front ends: the core is independent of its
delivery mechanism, and swapping the in-memory store for a database is a
one-line change in each composition root.