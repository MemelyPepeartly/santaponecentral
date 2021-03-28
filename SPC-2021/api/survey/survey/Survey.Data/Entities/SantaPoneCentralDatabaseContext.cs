using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Survey.Data.Entities
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

        public virtual DbSet<AssignmentStatus> AssignmentStatuses { get; set; }
        public virtual DbSet<BoardEntry> BoardEntries { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientRelationXref> ClientRelationXrefs { get; set; }
        public virtual DbSet<ClientStatus> ClientStatuses { get; set; }
        public virtual DbSet<ClientTagXref> ClientTagXrefs { get; set; }
        public virtual DbSet<EntryType> EntryTypes { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<SurveyOption> SurveyOptions { get; set; }
        public virtual DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public virtual DbSet<SurveyQuestionOptionXref> SurveyQuestionOptionXrefs { get; set; }
        public virtual DbSet<SurveyQuestionXref> SurveyQuestionXrefs { get; set; }
        public virtual DbSet<SurveyResponse> SurveyResponses { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<YuleLog> YuleLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AssignmentStatus>(entity =>
            {
                entity.ToTable("AssignmentStatus", "app");

                entity.HasIndex(e => e.AssignmentStatusName, "UQ__Assignme__216918BFF7CCE854")
                    .IsUnique();

                entity.HasIndex(e => e.AssignmentStatusDescription, "UQ__Assignme__9028EF9CBAD33D28")
                    .IsUnique();

                entity.Property(e => e.AssignmentStatusId)
                    .ValueGeneratedNever()
                    .HasColumnName("assignmentStatusID");

                entity.Property(e => e.AssignmentStatusDescription)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("assignmentStatusDescription");

                entity.Property(e => e.AssignmentStatusName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("assignmentStatusName");
            });

            modelBuilder.Entity<BoardEntry>(entity =>
            {
                entity.ToTable("BoardEntry", "app");

                entity.HasIndex(e => new { e.ThreadNumber, e.PostNumber }, "boardEntryID")
                    .IsUnique();

                entity.Property(e => e.BoardEntryId)
                    .ValueGeneratedNever()
                    .HasColumnName("boardEntryID");

                entity.Property(e => e.DateTimeEntered)
                    .HasColumnType("datetime")
                    .HasColumnName("dateTimeEntered");

                entity.Property(e => e.EntryTypeId).HasColumnName("entryTypeID");

                entity.Property(e => e.PostDescription)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("postDescription");

                entity.Property(e => e.PostNumber).HasColumnName("postNumber");

                entity.Property(e => e.ThreadNumber).HasColumnName("threadNumber");

                entity.HasOne(d => d.EntryType)
                    .WithMany(p => p.BoardEntries)
                    .HasForeignKey(d => d.EntryTypeId)
                    .HasConstraintName("FK__BoardEntr__entry__2767EA6E");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category", "app");

                entity.Property(e => e.CategoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("categoryID");

                entity.Property(e => e.CategoryDescription)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("categoryDescription");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("categoryName");
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("ChatMessage", "app");

                entity.Property(e => e.ChatMessageId)
                    .ValueGeneratedNever()
                    .HasColumnName("chatMessageID");

                entity.Property(e => e.ClientRelationXrefId).HasColumnName("clientRelationXrefID");

                entity.Property(e => e.DateTimeSent)
                    .HasColumnType("datetime")
                    .HasColumnName("dateTimeSent");

                entity.Property(e => e.FromAdmin).HasColumnName("fromAdmin");

                entity.Property(e => e.IsMessageRead).HasColumnName("isMessageRead");

                entity.Property(e => e.MessageContent)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("messageContent");

                entity.Property(e => e.MessageReceiverClientId).HasColumnName("messageReceiverClientID");

                entity.Property(e => e.MessageSenderClientId).HasColumnName("messageSenderClientID");

                entity.HasOne(d => d.ClientRelationXref)
                    .WithMany(p => p.ChatMessages)
                    .HasForeignKey(d => d.ClientRelationXrefId)
                    .HasConstraintName("FK__ChatMessa__clien__20BAECDF");

                entity.HasOne(d => d.MessageReceiverClient)
                    .WithMany(p => p.ChatMessageMessageReceiverClients)
                    .HasForeignKey(d => d.MessageReceiverClientId)
                    .HasConstraintName("FK__ChatMessa__messa__1FC6C8A6");

                entity.HasOne(d => d.MessageSenderClient)
                    .WithMany(p => p.ChatMessageMessageSenderClients)
                    .HasForeignKey(d => d.MessageSenderClientId)
                    .HasConstraintName("FK__ChatMessa__messa__1ED2A46D");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client", "app");

                entity.HasIndex(e => e.Email, "UQ__Client__AB6E6164129F84A0")
                    .IsUnique();

                entity.Property(e => e.ClientId)
                    .ValueGeneratedNever()
                    .HasColumnName("clientID");

                entity.Property(e => e.AddressLine1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("addressLine1");

                entity.Property(e => e.AddressLine2)
                    .HasMaxLength(50)
                    .HasColumnName("addressLine2");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("city");

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("clientName");

                entity.Property(e => e.ClientStatusId).HasColumnName("clientStatusID");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("country");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.HasAccount).HasColumnName("hasAccount");

                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(50)
                    .HasColumnName("nickname");

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("postalCode");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("state");

                entity.HasOne(d => d.ClientStatus)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.ClientStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Client__clientSt__7017B584");
            });

            modelBuilder.Entity<ClientRelationXref>(entity =>
            {
                entity.ToTable("ClientRelationXref", "app");

                entity.HasIndex(e => new { e.SenderClientId, e.RecipientClientId, e.EventTypeId }, "clientRelationXrefID")
                    .IsUnique();

                entity.Property(e => e.ClientRelationXrefId)
                    .ValueGeneratedNever()
                    .HasColumnName("clientRelationXrefID");

                entity.Property(e => e.AssignmentStatusId).HasColumnName("assignmentStatusID");

                entity.Property(e => e.EventTypeId).HasColumnName("eventTypeID");

                entity.Property(e => e.RecipientClientId).HasColumnName("recipientClientID");

                entity.Property(e => e.SenderClientId).HasColumnName("senderClientID");

                entity.HasOne(d => d.AssignmentStatus)
                    .WithMany(p => p.ClientRelationXrefs)
                    .HasForeignKey(d => d.AssignmentStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientRel__assig__004E1D4D");

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.ClientRelationXrefs)
                    .HasForeignKey(d => d.EventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientRel__event__7F59F914");

                entity.HasOne(d => d.RecipientClient)
                    .WithMany(p => p.ClientRelationXrefRecipientClients)
                    .HasForeignKey(d => d.RecipientClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientRel__recip__7E65D4DB");

                entity.HasOne(d => d.SenderClient)
                    .WithMany(p => p.ClientRelationXrefSenderClients)
                    .HasForeignKey(d => d.SenderClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientRel__sende__7D71B0A2");
            });

            modelBuilder.Entity<ClientStatus>(entity =>
            {
                entity.ToTable("ClientStatus", "app");

                entity.Property(e => e.ClientStatusId)
                    .ValueGeneratedNever()
                    .HasColumnName("clientStatusID");

                entity.Property(e => e.StatusDescription)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("statusDescription");
            });

            modelBuilder.Entity<ClientTagXref>(entity =>
            {
                entity.ToTable("ClientTagXref", "app");

                entity.HasIndex(e => new { e.ClientId, e.TagId }, "clientTagXrefID")
                    .IsUnique();

                entity.Property(e => e.ClientTagXrefId)
                    .ValueGeneratedNever()
                    .HasColumnName("clientTagXrefID");

                entity.Property(e => e.ClientId).HasColumnName("clientID");

                entity.Property(e => e.TagId).HasColumnName("tagID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientTagXrefs)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientTag__clien__1B021389");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.ClientTagXrefs)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientTag__tagID__1BF637C2");
            });

            modelBuilder.Entity<EntryType>(entity =>
            {
                entity.ToTable("EntryType", "app");

                entity.HasIndex(e => e.EntryTypeName, "UQ__EntryTyp__2E52653CDF86F02C")
                    .IsUnique();

                entity.Property(e => e.EntryTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("entryTypeID");

                entity.Property(e => e.AdminOnly).HasColumnName("adminOnly");

                entity.Property(e => e.EntryTypeDescription)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("entryTypeDescription");

                entity.Property(e => e.EntryTypeName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("entryTypeName");
            });

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.ToTable("EventType", "app");

                entity.HasIndex(e => e.EventDescription, "UQ__EventTyp__F516F46EC5DE03BB")
                    .IsUnique();

                entity.Property(e => e.EventTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("eventTypeID");

                entity.Property(e => e.EventDescription)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("eventDescription");

                entity.Property(e => e.IsActive).HasColumnName("isActive");
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.ToTable("Note", "app");

                entity.Property(e => e.NoteId)
                    .ValueGeneratedNever()
                    .HasColumnName("noteID");

                entity.Property(e => e.ClientId).HasColumnName("clientID");

                entity.Property(e => e.NoteContents)
                    .HasMaxLength(2000)
                    .HasColumnName("noteContents");

                entity.Property(e => e.NoteSubject)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("noteSubject");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Note__clientID__72F4222F");
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.ToTable("Survey", "app");

                entity.Property(e => e.SurveyId)
                    .ValueGeneratedNever()
                    .HasColumnName("surveyID");

                entity.Property(e => e.EventTypeId).HasColumnName("eventTypeID");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.SurveyDescription)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("surveyDescription");

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.Surveys)
                    .HasForeignKey(d => d.EventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Survey__eventTyp__032A89F8");
            });

            modelBuilder.Entity<SurveyOption>(entity =>
            {
                entity.ToTable("SurveyOption", "app");

                entity.Property(e => e.SurveyOptionId)
                    .ValueGeneratedNever()
                    .HasColumnName("surveyOptionID");

                entity.Property(e => e.DisplayText)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("displayText");

                entity.Property(e => e.SurveyOptionValue)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("surveyOptionValue");
            });

            modelBuilder.Entity<SurveyQuestion>(entity =>
            {
                entity.ToTable("SurveyQuestion", "app");

                entity.Property(e => e.SurveyQuestionId)
                    .ValueGeneratedNever()
                    .HasColumnName("surveyQuestionID");

                entity.Property(e => e.IsSurveyOptionList).HasColumnName("isSurveyOptionList");

                entity.Property(e => e.QuestionText)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("questionText");

                entity.Property(e => e.SenderCanView).HasColumnName("senderCanView");
            });

            modelBuilder.Entity<SurveyQuestionOptionXref>(entity =>
            {
                entity.ToTable("SurveyQuestionOptionXref", "app");

                entity.Property(e => e.SurveyQuestionOptionXrefId)
                    .ValueGeneratedNever()
                    .HasColumnName("surveyQuestionOptionXrefID");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.SortOrder).HasColumnName("sortOrder");

                entity.Property(e => e.SurveyOptionId).HasColumnName("surveyOptionID");

                entity.Property(e => e.SurveyQuestionId).HasColumnName("surveyQuestionID");

                entity.HasOne(d => d.SurveyOption)
                    .WithMany(p => p.SurveyQuestionOptionXrefs)
                    .HasForeignKey(d => d.SurveyOptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__145515FA");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyQuestionOptionXrefs)
                    .HasForeignKey(d => d.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__1360F1C1");
            });

            modelBuilder.Entity<SurveyQuestionXref>(entity =>
            {
                entity.ToTable("SurveyQuestionXref", "app");

                entity.Property(e => e.SurveyQuestionXrefId)
                    .ValueGeneratedNever()
                    .HasColumnName("surveyQuestionXrefID");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.SortOrder).HasColumnName("sortOrder");

                entity.Property(e => e.SurveyId).HasColumnName("surveyID");

                entity.Property(e => e.SurveyQuestionId).HasColumnName("surveyQuestionID");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyQuestionXrefs)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__0F9060DD");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyQuestionXrefs)
                    .HasForeignKey(d => d.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__10848516");
            });

            modelBuilder.Entity<SurveyResponse>(entity =>
            {
                entity.ToTable("SurveyResponse", "app");

                entity.Property(e => e.SurveyResponseId)
                    .ValueGeneratedNever()
                    .HasColumnName("surveyResponseID");

                entity.Property(e => e.ClientId).HasColumnName("clientID");

                entity.Property(e => e.ResponseText)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .HasColumnName("responseText");

                entity.Property(e => e.SurveyId).HasColumnName("surveyID");

                entity.Property(e => e.SurveyOptionId).HasColumnName("surveyOptionID");

                entity.Property(e => e.SurveyQuestionId).HasColumnName("surveyQuestionID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.SurveyResponses)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyRes__clien__0ACBABC0");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyResponses)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyRes__surve__09D78787");

                entity.HasOne(d => d.SurveyOption)
                    .WithMany(p => p.SurveyResponses)
                    .HasForeignKey(d => d.SurveyOptionId)
                    .HasConstraintName("FK__SurveyRes__surve__0CB3F432");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyResponses)
                    .HasForeignKey(d => d.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyRes__surve__0BBFCFF9");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag", "app");

                entity.HasIndex(e => e.TagName, "UQ__Tag__288C38517BC5F456")
                    .IsUnique();

                entity.Property(e => e.TagId)
                    .ValueGeneratedNever()
                    .HasColumnName("tagID");

                entity.Property(e => e.TagName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("tagName");
            });

            modelBuilder.Entity<YuleLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__YuleLog__7839F62D2EC22B91");

                entity.ToTable("YuleLog", "app");

                entity.Property(e => e.LogId)
                    .ValueGeneratedNever()
                    .HasColumnName("logID");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.LogDate)
                    .HasColumnType("datetime")
                    .HasColumnName("logDate");

                entity.Property(e => e.LogText)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("logText");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.YuleLogs)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__YuleLog__categor__2C2C9F8B");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
