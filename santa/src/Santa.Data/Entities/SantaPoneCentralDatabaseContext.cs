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

        public virtual DbSet<ChatMessage> ChatMessage { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientRelationXref> ClientRelationXref { get; set; }
        public virtual DbSet<ClientStatus> ClientStatus { get; set; }
        public virtual DbSet<ClientTagXref> ClientTagXref { get; set; }
        public virtual DbSet<EventType> EventType { get; set; }
        public virtual DbSet<Survey> Survey { get; set; }
        public virtual DbSet<SurveyOption> SurveyOption { get; set; }
        public virtual DbSet<SurveyQuestion> SurveyQuestion { get; set; }
        public virtual DbSet<SurveyQuestionOptionXref> SurveyQuestionOptionXref { get; set; }
        public virtual DbSet<SurveyQuestionXref> SurveyQuestionXref { get; set; }
        public virtual DbSet<SurveyResponse> SurveyResponse { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("ChatMessage", "app");

                entity.Property(e => e.ChatMessageId)
                    .HasColumnName("chatMessageID")
                    .HasViewColumnName("chatMessageID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ClientRelationXrefId)
                    .HasColumnName("clientRelationXrefID")
                    .HasViewColumnName("clientRelationXrefID");

                entity.Property(e => e.DateTimeSent)
                    .HasColumnName("dateTimeSent")
                    .HasViewColumnName("dateTimeSent")
                    .HasColumnType("datetime");

                entity.Property(e => e.IsMessageRead)
                    .HasColumnName("isMessageRead")
                    .HasViewColumnName("isMessageRead");

                entity.Property(e => e.MessageContent)
                    .IsRequired()
                    .HasColumnName("messageContent")
                    .HasViewColumnName("messageContent")
                    .HasMaxLength(1000);

                entity.Property(e => e.MessageReceiverClientId)
                    .HasColumnName("messageReceiverClientID")
                    .HasViewColumnName("messageReceiverClientID");

                entity.Property(e => e.MessageSenderClientId)
                    .HasColumnName("messageSenderClientID")
                    .HasViewColumnName("messageSenderClientID");

                entity.HasOne(d => d.ClientRelationXref)
                    .WithMany(p => p.ChatMessage)
                    .HasForeignKey(x => x.ClientRelationXrefId)
                    .HasConstraintName("FK__ChatMessa__clien__38A78509");

                entity.HasOne(d => d.MessageReceiverClient)
                    .WithMany(p => p.ChatMessageMessageReceiverClient)
                    .HasForeignKey(x => x.MessageReceiverClientId)
                    .HasConstraintName("FK__ChatMessa__messa__37B360D0");

                entity.HasOne(d => d.MessageSenderClient)
                    .WithMany(p => p.ChatMessageMessageSenderClient)
                    .HasForeignKey(x => x.MessageSenderClientId)
                    .HasConstraintName("FK__ChatMessa__messa__36BF3C97");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client", "app");

                entity.HasIndex(x => x.Email)
                    .HasName("UQ__Client__AB6E61649FA65454")
                    .IsUnique();

                entity.Property(e => e.ClientId)
                    .HasColumnName("clientID")
                    .HasViewColumnName("clientID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddressLine1)
                    .IsRequired()
                    .HasColumnName("addressLine1")
                    .HasViewColumnName("addressLine1")
                    .HasMaxLength(50);

                entity.Property(e => e.AddressLine2)
                    .HasColumnName("addressLine2")
                    .HasViewColumnName("addressLine2")
                    .HasMaxLength(50);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasViewColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasColumnName("clientName")
                    .HasViewColumnName("clientName")
                    .HasMaxLength(50);

                entity.Property(e => e.ClientStatusId)
                    .HasColumnName("clientStatusID")
                    .HasViewColumnName("clientStatusID");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasViewColumnName("country")
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasViewColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Nickname)
                    .HasColumnName("nickname")
                    .HasViewColumnName("nickname")
                    .HasMaxLength(50);

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasColumnName("postalCode")
                    .HasViewColumnName("postalCode")
                    .HasMaxLength(25);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasViewColumnName("state")
                    .HasMaxLength(50);

                entity.HasOne(d => d.ClientStatus)
                    .WithMany(p => p.Client)
                    .HasForeignKey(x => x.ClientStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Client__clientSt__0FA56F76");
            });

            modelBuilder.Entity<ClientRelationXref>(entity =>
            {
                entity.ToTable("ClientRelationXref", "app");

                entity.HasIndex(x => new { x.SenderClientId, x.RecipientClientId, x.EventTypeId })
                    .HasName("clientRelationXrefID")
                    .IsUnique();

                entity.Property(e => e.ClientRelationXrefId)
                    .HasColumnName("clientRelationXrefID")
                    .HasViewColumnName("clientRelationXrefID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Completed)
                    .HasColumnName("completed")
                    .HasViewColumnName("completed");

                entity.Property(e => e.EventTypeId)
                    .HasColumnName("eventTypeID")
                    .HasViewColumnName("eventTypeID");

                entity.Property(e => e.RecipientClientId)
                    .HasColumnName("recipientClientID")
                    .HasViewColumnName("recipientClientID");

                entity.Property(e => e.SenderClientId)
                    .HasColumnName("senderClientID")
                    .HasViewColumnName("senderClientID");

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.ClientRelationXref)
                    .HasForeignKey(x => x.EventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientRel__event__183AB577");

                entity.HasOne(d => d.RecipientClient)
                    .WithMany(p => p.ClientRelationXrefRecipientClient)
                    .HasForeignKey(x => x.RecipientClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientRel__recip__1746913E");

                entity.HasOne(d => d.SenderClient)
                    .WithMany(p => p.ClientRelationXrefSenderClient)
                    .HasForeignKey(x => x.SenderClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientRel__sende__16526D05");
            });

            modelBuilder.Entity<ClientStatus>(entity =>
            {
                entity.ToTable("ClientStatus", "app");

                entity.Property(e => e.ClientStatusId)
                    .HasColumnName("clientStatusID")
                    .HasViewColumnName("clientStatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.StatusDescription)
                    .IsRequired()
                    .HasColumnName("statusDescription")
                    .HasViewColumnName("statusDescription")
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<ClientTagXref>(entity =>
            {
                entity.ToTable("ClientTagXref", "app");

                entity.HasIndex(x => new { x.ClientId, x.TagId })
                    .HasName("clientTagXrefID")
                    .IsUnique();

                entity.Property(e => e.ClientTagXrefId)
                    .HasColumnName("clientTagXrefID")
                    .HasViewColumnName("clientTagXrefID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ClientId)
                    .HasColumnName("clientID")
                    .HasViewColumnName("clientID");

                entity.Property(e => e.TagId)
                    .HasColumnName("tagID")
                    .HasViewColumnName("tagID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientTagXref)
                    .HasForeignKey(x => x.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientTag__clien__32EEABB3");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.ClientTagXref)
                    .HasForeignKey(x => x.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ClientTag__tagID__33E2CFEC");
            });

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.ToTable("EventType", "app");

                entity.HasIndex(x => x.EventDescription)
                    .HasName("UQ__EventTyp__F516F46E91E53716")
                    .IsUnique();

                entity.Property(e => e.EventTypeId)
                    .HasColumnName("eventTypeID")
                    .HasViewColumnName("eventTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EventDescription)
                    .IsRequired()
                    .HasColumnName("eventDescription")
                    .HasViewColumnName("eventDescription")
                    .HasMaxLength(100);

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasViewColumnName("isActive");
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.ToTable("Survey", "app");

                entity.Property(e => e.SurveyId)
                    .HasColumnName("surveyID")
                    .HasViewColumnName("surveyID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EventTypeId)
                    .HasColumnName("eventTypeID")
                    .HasViewColumnName("eventTypeID");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasViewColumnName("isActive");

                entity.Property(e => e.SurveyDescription)
                    .IsRequired()
                    .HasColumnName("surveyDescription")
                    .HasViewColumnName("surveyDescription")
                    .HasMaxLength(100);

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.Survey)
                    .HasForeignKey(x => x.EventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Survey__eventTyp__1B172222");
            });

            modelBuilder.Entity<SurveyOption>(entity =>
            {
                entity.ToTable("SurveyOption", "app");

                entity.Property(e => e.SurveyOptionId)
                    .HasColumnName("surveyOptionID")
                    .HasViewColumnName("surveyOptionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DisplayText)
                    .IsRequired()
                    .HasColumnName("displayText")
                    .HasViewColumnName("displayText")
                    .HasMaxLength(100);

                entity.Property(e => e.SurveyOptionValue)
                    .IsRequired()
                    .HasColumnName("surveyOptionValue")
                    .HasViewColumnName("surveyOptionValue")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SurveyQuestion>(entity =>
            {
                entity.ToTable("SurveyQuestion", "app");

                entity.Property(e => e.SurveyQuestionId)
                    .HasColumnName("surveyQuestionID")
                    .HasViewColumnName("surveyQuestionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsSurveyOptionList)
                    .HasColumnName("isSurveyOptionList")
                    .HasViewColumnName("isSurveyOptionList");

                entity.Property(e => e.QuestionText)
                    .IsRequired()
                    .HasColumnName("questionText")
                    .HasViewColumnName("questionText")
                    .HasMaxLength(150);

                entity.Property(e => e.SenderCanView)
                    .HasColumnName("senderCanView")
                    .HasViewColumnName("senderCanView");
            });

            modelBuilder.Entity<SurveyQuestionOptionXref>(entity =>
            {
                entity.ToTable("SurveyQuestionOptionXref", "app");

                entity.Property(e => e.SurveyQuestionOptionXrefId)
                    .HasColumnName("surveyQuestionOptionXrefID")
                    .HasViewColumnName("surveyQuestionOptionXrefID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasViewColumnName("isActive");

                entity.Property(e => e.SortOrder)
                    .IsRequired()
                    .HasColumnName("sortOrder")
                    .HasViewColumnName("sortOrder")
                    .HasMaxLength(5);

                entity.Property(e => e.SurveyOptionId)
                    .HasColumnName("surveyOptionID")
                    .HasViewColumnName("surveyOptionID");

                entity.Property(e => e.SurveyQuestionId)
                    .HasColumnName("surveyQuestionID")
                    .HasViewColumnName("surveyQuestionID");

                entity.HasOne(d => d.SurveyOption)
                    .WithMany(p => p.SurveyQuestionOptionXref)
                    .HasForeignKey(x => x.SurveyOptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__2C41AE24");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyQuestionOptionXref)
                    .HasForeignKey(x => x.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__2B4D89EB");
            });

            modelBuilder.Entity<SurveyQuestionXref>(entity =>
            {
                entity.ToTable("SurveyQuestionXref", "app");

                entity.Property(e => e.SurveyQuestionXrefId)
                    .HasColumnName("surveyQuestionXrefID")
                    .HasViewColumnName("surveyQuestionXrefID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasViewColumnName("isActive");

                entity.Property(e => e.SortOrder)
                    .IsRequired()
                    .HasColumnName("sortOrder")
                    .HasViewColumnName("sortOrder")
                    .HasMaxLength(5);

                entity.Property(e => e.SurveyId)
                    .HasColumnName("surveyID")
                    .HasViewColumnName("surveyID");

                entity.Property(e => e.SurveyQuestionId)
                    .HasColumnName("surveyQuestionID")
                    .HasViewColumnName("surveyQuestionID");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyQuestionXref)
                    .HasForeignKey(x => x.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__277CF907");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyQuestionXref)
                    .HasForeignKey(x => x.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyQue__surve__28711D40");
            });

            modelBuilder.Entity<SurveyResponse>(entity =>
            {
                entity.ToTable("SurveyResponse", "app");

                entity.Property(e => e.SurveyResponseId)
                    .HasColumnName("surveyResponseID")
                    .HasViewColumnName("surveyResponseID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ClientId)
                    .HasColumnName("clientID")
                    .HasViewColumnName("clientID");

                entity.Property(e => e.ResponseText)
                    .IsRequired()
                    .HasColumnName("responseText")
                    .HasViewColumnName("responseText")
                    .HasMaxLength(2000);

                entity.Property(e => e.SurveyId)
                    .HasColumnName("surveyID")
                    .HasViewColumnName("surveyID");

                entity.Property(e => e.SurveyOptionId)
                    .HasColumnName("surveyOptionID")
                    .HasViewColumnName("surveyOptionID");

                entity.Property(e => e.SurveyQuestionId)
                    .HasColumnName("surveyQuestionID")
                    .HasViewColumnName("surveyQuestionID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(x => x.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyRes__clien__22B843EA");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(x => x.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyRes__surve__21C41FB1");

                entity.HasOne(d => d.SurveyOption)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(x => x.SurveyOptionId)
                    .HasConstraintName("FK__SurveyRes__surve__24A08C5C");

                entity.HasOne(d => d.SurveyQuestion)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(x => x.SurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SurveyRes__surve__23AC6823");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag", "app");

                entity.HasIndex(x => x.TagName)
                    .HasName("UQ__Tag__288C385186768984")
                    .IsUnique();

                entity.Property(e => e.TagId)
                    .HasColumnName("tagID")
                    .HasViewColumnName("tagID")
                    .ValueGeneratedNever();

                entity.Property(e => e.TagName)
                    .IsRequired()
                    .HasColumnName("tagName")
                    .HasViewColumnName("tagName")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
