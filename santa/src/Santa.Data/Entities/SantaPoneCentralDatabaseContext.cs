using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Santa.Data.Entities
{
    public partial class SantaPoneCentralDatabaseContext : DbContext
    {
        public SantaPoneCentralDatabaseContext()
        {
        }

        public SantaPoneCentralDatabaseContext(DbContextOptions<SantaPoneCentralDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientRelationXref> ClientRelationXref { get; set; }
        public virtual DbSet<ClientStatus> ClientStatus { get; set; }
        public virtual DbSet<EventType> EventType { get; set; }
        public virtual DbSet<Survey> Survey { get; set; }
        public virtual DbSet<SurveyOption> SurveyOption { get; set; }
        public virtual DbSet<SurveyQuestion> SurveyQuestion { get; set; }
        public virtual DbSet<SurveyQuestionOptionXref> SurveyQuestionOptionXref { get; set; }
        public virtual DbSet<SurveyQuestionXref> SurveyQuestionXref { get; set; }
        public virtual DbSet<SurveyResponse> SurveyResponse { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client", "app");

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Client__AB6E6164EE37C09F")
                    .IsUnique();

                entity.Property(e => e.ClientId)
                    .HasColumnName("clientID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddressLine1)
                    .IsRequired()
                    .HasColumnName("addressLine1")
                    .HasMaxLength(50);

                entity.Property(e => e.AddressLine2)
                    .HasColumnName("addressLine2")
                    .HasMaxLength(50);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasColumnName("clientName")
                    .HasMaxLength(50);

                entity.Property(e => e.ClientStatusId).HasColumnName("clientStatusID");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Nickname)
                    .HasColumnName("nickname")
                    .HasMaxLength(50);

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasColumnName("postalCode")
                    .HasMaxLength(25);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasMaxLength(50);

                entity.HasOne(d => d.ClientStatus)
                    .WithMany(p => p.Client)
                    .HasForeignKey(d => d.ClientStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Client__clientSt__2215F810");
            });

            modelBuilder.Entity<ClientRelationXref>(entity =>
            {
                entity.ToTable("ClientRelationXref", "app");

                entity.Property(e => e.ClientRelationXrefId).HasColumnName("clientRelationXrefID");

                entity.Property(e => e.EventTypeId).HasColumnName("eventTypeID");

                entity.Property(e => e.RecipientClientId).HasColumnName("recipientClientID");

                entity.Property(e => e.SenderClientId).HasColumnName("senderClientID");

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.ClientRelationXref)
                    .HasForeignKey(d => d.EventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientRel__event__29B719D8");

                entity.HasOne(d => d.RecipientClient)
                    .WithMany(p => p.ClientRelationXrefRecipientClient)
                    .HasForeignKey(d => d.RecipientClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientRel__recip__28C2F59F");

                entity.HasOne(d => d.SenderClient)
                    .WithMany(p => p.ClientRelationXrefSenderClient)
                    .HasForeignKey(d => d.SenderClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientRel__sende__27CED166");
            });

            modelBuilder.Entity<ClientStatus>(entity =>
            {
                entity.ToTable("ClientStatus", "app");

                entity.Property(e => e.ClientStatusId)
                    .HasColumnName("clientStatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.StatusDescription)
                    .IsRequired()
                    .HasColumnName("statusDescription")
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.ToTable("EventType", "app");

                entity.HasIndex(e => e.EventDescription)
                    .HasName("UQ__EventTyp__F516F46E3CFA98BB")
                    .IsUnique();

                entity.Property(e => e.EventTypeId)
                    .HasColumnName("eventTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EventDescription)
                    .IsRequired()
                    .HasColumnName("eventDescription")
                    .HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnName("isActive");
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.ToTable("Survey", "app");

                entity.Property(e => e.SurveyId)
                    .HasColumnName("surveyID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EventTypeId).HasColumnName("eventTypeID");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.SurveyDescription)
                    .IsRequired()
                    .HasColumnName("surveyDescription")
                    .HasMaxLength(100);

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.Survey)
                    .HasForeignKey(d => d.EventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Survey__eventTyp__2C938683");
            });

            modelBuilder.Entity<SurveyOption>(entity =>
            {
                entity.ToTable("SurveyOption", "app");

                entity.Property(e => e.SurveyOptionId)
                    .HasColumnName("surveyOptionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DisplayText)
                    .IsRequired()
                    .HasColumnName("displayText")
                    .HasMaxLength(100);

                entity.Property(e => e.SurveyOptionValue)
                    .IsRequired()
                    .HasColumnName("surveyOptionValue")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SurveyQuestion>(entity =>
            {
                entity.ToTable("SurveyQuestion", "app");

                entity.Property(e => e.SurveyQuestionId)
                    .HasColumnName("surveyQuestionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsSurveyOptionList).HasColumnName("isSurveyOptionList");

                entity.Property(e => e.QuestionText)
                    .IsRequired()
                    .HasColumnName("questionText")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<SurveyQuestionOptionXref>(entity =>
            {
                entity.HasKey(e => e.SurveyQuestionOptionXref1)
                    .HasName("PK__SurveyQu__1BAA3BB1EC55B2B1");

                entity.ToTable("SurveyQuestionOptionXref", "app");

                entity.Property(e => e.SurveyQuestionOptionXref1).HasColumnName("surveyQuestionOptionXref");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.SortOrder)
                    .IsRequired()
                    .HasColumnName("sortOrder")
                    .HasMaxLength(5);

                entity.Property(e => e.SurveyOptionId).HasColumnName("surveyOptionID");

                entity.Property(e => e.SurveyQuestionId).HasColumnName("surveyQuestionID");

                entity.HasOne(d => d.SurveyOption)
                    .WithMany(p => p.SurveyQuestionOptionXref)
                    .HasForeignKey(d => d.SurveyOptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__3DBE1285");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyQuestionOptionXref)
                    .HasForeignKey(d => d.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__3CC9EE4C");
            });

            modelBuilder.Entity<SurveyQuestionXref>(entity =>
            {
                entity.ToTable("SurveyQuestionXref", "app");

                entity.Property(e => e.SurveyQuestionXrefId).HasColumnName("surveyQuestionXrefID");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.SortOrder)
                    .IsRequired()
                    .HasColumnName("sortOrder")
                    .HasMaxLength(5);

                entity.Property(e => e.SurveyId).HasColumnName("surveyID");

                entity.Property(e => e.SurveyQuestionId).HasColumnName("surveyQuestionID");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyQuestionXref)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__38F95D68");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyQuestionXref)
                    .HasForeignKey(d => d.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__39ED81A1");
            });

            modelBuilder.Entity<SurveyResponse>(entity =>
            {
                entity.ToTable("SurveyResponse", "app");

                entity.Property(e => e.SurveyResponseId)
                    .HasColumnName("surveyResponseID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ClientId).HasColumnName("clientID");

                entity.Property(e => e.ResponseText)
                    .IsRequired()
                    .HasColumnName("responseText")
                    .HasMaxLength(150);

                entity.Property(e => e.SurveyId).HasColumnName("surveyID");

                entity.Property(e => e.SurveyOptionId).HasColumnName("surveyOptionID");

                entity.Property(e => e.SurveyQuestionId).HasColumnName("surveyQuestionID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyRes__clien__3434A84B");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyRes__surve__33408412");

                entity.HasOne(d => d.SurveyOption)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(d => d.SurveyOptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyRes__surve__361CF0BD");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(d => d.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyRes__surve__3528CC84");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
