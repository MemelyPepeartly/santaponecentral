using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Data.Entities
{
    public partial class SantaPoneCentralDatabaseContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ClientRelationXref>()
                .HasOne(crx => crx.SenderClient)
                .WithMany(c => c.ClientRelationXrefSenderClient)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientRelationXref>()
                .HasOne(crx => crx.RecipientClient)
                .WithMany(c => c.ClientRelationXrefRecipientClient)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientRelationXref>()
                .HasOne(crx => crx.RecipientClient)
                .WithMany(c => c.ClientRelationXrefRecipientClient)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientTagXref>()
                .HasOne(ctx => ctx.Client)
                .WithMany(t => t.ClientTagXref)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SurveyResponse>()
                .HasOne(d => d.Client)
                .WithMany(p => p.SurveyResponse)
                .OnDelete(DeleteBehavior.Cascade);
#warning Need a thing here for deleting event types and all that jazz
        }
    }
}
