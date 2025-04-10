using System;
using System.Collections.Generic;
using BookingSite.Domains.EntitiesDB;
using Microsoft.EntityFrameworkCore;

namespace BookingSite.Domains.Context;

public partial class BookingsDBContext : DbContext
{
    public BookingsDBContext()
    {
    }

    public BookingsDBContext(DbContextOptions<BookingsDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<MealChoice> MealChoices { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<RouteSegment> RouteSegments { get; set; }

    public virtual DbSet<Season> Seasons { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TravelClass> TravelClasses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=BookingsDB;TrustServerCertificate=True;MultipleActiveResultSets=True;User=SA;Password=Rootpassword123_123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Airports__3214EC0705DD149F");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.City).WithMany(p => p.Airports)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__Airports__CityId__2A164134");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PK__Bookings__6B0A6BBE077060BA");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__Bookings__Ticket__47A6A41B");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Bookings__UserId__46B27FE2");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cities__3214EC070A98C750");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Flights__3214EC074EFE3197");

            entity.HasOne(d => d.Route).WithMany(p => p.Flights)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("FK__Flights__RouteId__3C34F16F");
        });

        modelBuilder.Entity<MealChoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MealChoi__3214EC07EE4AD5C3");

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Airport).WithMany(p => p.MealChoices)
                .HasForeignKey(d => d.AirportId)
                .HasConstraintName("FK__MealChoic__Airpo__3F115E1A");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Routes__3214EC078F424933");

            entity.HasOne(d => d.FromAirport).WithMany(p => p.RouteFromAirports)
                .HasForeignKey(d => d.FromAirportId)
                .HasConstraintName("FK__Routes__FromAirp__3493CFA7");

            entity.HasOne(d => d.ToAirport).WithMany(p => p.RouteToAirports)
                .HasForeignKey(d => d.ToAirportId)
                .HasConstraintName("FK__Routes__ToAirpor__3587F3E0");
        });

        modelBuilder.Entity<RouteSegment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RouteSeg__3214EC07BF839F08");

            entity.HasOne(d => d.Airport).WithMany(p => p.RouteSegments)
                .HasForeignKey(d => d.AirportId)
                .HasConstraintName("FK__RouteSegm__Airpo__395884C4");

            entity.HasOne(d => d.Route).WithMany(p => p.RouteSegments)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("FK__RouteSegm__Route__3864608B");
        });

        modelBuilder.Entity<Season>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Seasons__3214EC07ABFEADD9");

            entity.HasOne(d => d.Airport).WithMany(p => p.Seasons)
                .HasForeignKey(d => d.AirportId)
                .HasConstraintName("FK__Seasons__Airport__2CF2ADDF");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Seats__3214EC07303D93A1");

            entity.Property(e => e.SeatNumber)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.TravelClass).WithMany(p => p.Seats)
                .HasForeignKey(d => d.TravelClassId)
                .HasConstraintName("FK__Seats__TravelCla__31B762FC");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tickets__3214EC0705CCB4DD");

            entity.HasOne(d => d.Flight).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.FlightId)
                .HasConstraintName("FK__Tickets__FlightI__41EDCAC5");

            entity.HasOne(d => d.Meal).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.MealId)
                .HasConstraintName("FK__Tickets__MealId__42E1EEFE");

            entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatId)
                .HasConstraintName("FK__Tickets__SeatId__43D61337");
        });

        modelBuilder.Entity<TravelClass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TravelCl__3214EC079AB0C82B");

            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0781FB9F80");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
