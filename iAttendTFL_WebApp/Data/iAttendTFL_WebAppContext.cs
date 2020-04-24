using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using iAttendTFL_WebApp.Models;
using BarcodeLib;

namespace iAttendTFL_WebApp.Data
{
    public class iAttendTFL_WebAppContext : DbContext
    {
        public iAttendTFL_WebAppContext (DbContextOptions<iAttendTFL_WebAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            //modelBuilder.Entity<account_attendance>()
            //    .HasKey(c => new { c.account_id, c.scan_event_id });
            //modelBuilder.Entity<account_attendance>()
            //    .HasOne(c => c.account)
            //    .WithMany(t => t.account_attendances)
            //    .HasForeignKey(c => c.account_id);
            //modelBuilder.Entity<account_attendance>()
            //    .HasOne(c => c.scan_event)
            //    .WithMany(t => t.account_attendances)
            //    .HasForeignKey(c => c.scan_event_id);

            //modelBuilder.Entity<track_requirement>()
            //    .HasKey(c => new { c.track_id, c.requirement_id });
            //modelBuilder.Entity<track_requirement>()
            //    .HasOne(c => c.track)
            //    .WithMany(t => t.track_requirements)
            //    .HasForeignKey(c => c.track_id);
            //modelBuilder.Entity<track_requirement>()
            //    .HasOne(c => c.requirement)
            //    .WithMany(t => t.track_requirements)
            //    .HasForeignKey(c => c.requirement_id);

            //modelBuilder.Entity<time_frame_track>()
            //    .HasKey(c => new { c.track_id, c.time_frame_id });
            //modelBuilder.Entity<time_frame_track>()
            //    .HasOne(c => c.time_frame)
            //    .WithMany(t => t.time_frame_tracks)
            //    .HasForeignKey(c => c.time_frame_id);
            //modelBuilder.Entity<time_frame_track>()
            //    .HasOne(c => c.track)
            //    .WithMany(t => t.time_frame_tracks)
            //    .HasForeignKey(c => c.track_id);

            //modelBuilder.Entity<event_requirement>()
            //    .HasKey(c => new { c.requirement_id, c.scan_event_id });
            //modelBuilder.Entity<event_requirement>()
            //    .HasOne(c => c.requirement)
            //    .WithMany(t => t.event_requirements)
            //    .HasForeignKey(c => c.requirement_id);
            //modelBuilder.Entity<event_requirement>()
            //    .HasOne(c => c.scan_event)
            //    .WithMany(t => t.event_requirements)
            //    .HasForeignKey(c => c.scan_event_id);

            //modelBuilder.Entity<token>()
            //    .HasKey(c => c.token_hash);
            //modelBuilder.Entity<token>()
            //    .HasOne(c => c.account)
            //    .WithMany(t => t.tokens)
            //    .HasForeignKey(c => c.account_id);

        }


        public DbSet<iAttendTFL_WebApp.Models.account> account { get; set; }
        
        

    }
}
