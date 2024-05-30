﻿// <auto-generated />
using System;
using InternalPortal.Infrastucture.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InternalPortal.Infrastucture.Migrations
{
    [DbContext(typeof(InternalPortalContext))]
    [Migration("20240213094304_AddTestScoreTable")]
    partial class AddTestScoreTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("InternalPortal.Core.Models.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)");

                    b.Property<string>("UserSid")
                        .IsRequired()
                        .HasMaxLength(63)
                        .HasColumnType("character varying(63)");

                    b.HasKey("Id");

                    b.ToTable("Profiles", "dbo");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TestTopicId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId");

                    b.HasIndex("TestTopicId");

                    b.ToTable("Tests", "test");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.TestQuestionAnswers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AnswerText")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<bool>("IsActual")
                        .HasColumnType("boolean");

                    b.Property<bool>("Meaning")
                        .HasColumnType("boolean");

                    b.Property<int>("TestQuestionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TestQuestionId");

                    b.ToTable("TestQuestionAnswers", "test");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.TestQuestions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActual")
                        .HasColumnType("boolean");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<int>("TestTopicId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TestTopicId");

                    b.ToTable("TestQuestions", "test");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.TestScore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId")
                        .IsUnique();

                    b.ToTable("TestsScore", "test");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.TestTopics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActual")
                        .HasColumnType("boolean");

                    b.Property<string>("TopicName")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.ToTable("TestTopics", "test");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.TestsAnswers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AnswerId")
                        .HasColumnType("integer");

                    b.Property<int>("TestId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("TestId");

                    b.ToTable("TestsAnswers", "test");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.Test", b =>
                {
                    b.HasOne("InternalPortal.Core.Models.Profile", "Profile")
                        .WithMany("Tests")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("InternalPortal.Core.Models.TestTopics", "TestTopic")
                        .WithMany("Tests")
                        .HasForeignKey("TestTopicId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Profile");

                    b.Navigation("TestTopic");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.TestQuestionAnswers", b =>
                {
                    b.HasOne("InternalPortal.Core.Models.TestQuestions", "TestQuestion")
                        .WithMany("TestAnswers")
                        .HasForeignKey("TestQuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("TestQuestion");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.TestQuestions", b =>
                {
                    b.HasOne("InternalPortal.Core.Models.TestTopics", "TestTopic")
                        .WithMany("TestQuestions")
                        .HasForeignKey("TestTopicId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("TestTopic");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.TestScore", b =>
                {
                    b.HasOne("InternalPortal.Core.Models.Profile", "Profile")
                        .WithOne("TestScore")
                        .HasForeignKey("InternalPortal.Core.Models.TestScore", "ProfileId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.TestsAnswers", b =>
                {
                    b.HasOne("InternalPortal.Core.Models.TestQuestionAnswers", "TestQuestionAnswer")
                        .WithMany("TestsAnswers")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("InternalPortal.Core.Models.Test", "Test")
                        .WithMany("TestsAnswers")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Test");

                    b.Navigation("TestQuestionAnswer");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.Profile", b =>
                {
                    b.Navigation("TestScore")
                        .IsRequired();

                    b.Navigation("Tests");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.Test", b =>
                {
                    b.Navigation("TestsAnswers");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.TestQuestionAnswers", b =>
                {
                    b.Navigation("TestsAnswers");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.TestQuestions", b =>
                {
                    b.Navigation("TestAnswers");
                });

            modelBuilder.Entity("InternalPortal.Core.Models.TestTopics", b =>
                {
                    b.Navigation("TestQuestions");

                    b.Navigation("Tests");
                });
#pragma warning restore 612, 618
        }
    }
}
