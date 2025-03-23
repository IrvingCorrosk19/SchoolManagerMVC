﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SchoolManager.Models;

#nullable disable

namespace SchoolManager.Migrations
{
    [DbContext(typeof(SchoolDbContext))]
    partial class SchoolDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SchoolManager.Models.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("PdfUrl")
                        .HasColumnType("text")
                        .HasColumnName("pdf_url");

                    b.Property<Guid?>("SchoolId")
                        .HasColumnType("uuid")
                        .HasColumnName("school_id");

                    b.Property<Guid?>("SubjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("subject_id");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("uuid")
                        .HasColumnName("teacher_id");

                    b.Property<string>("Trimester")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)")
                        .HasColumnName("trimester");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("activities_pkey");

                    b.HasIndex("GroupId");

                    b.HasIndex("SchoolId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("activities", (string)null);
                });

            modelBuilder.Entity("SchoolManager.Models.Attendance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("status");

                    b.Property<Guid?>("StudentId")
                        .HasColumnType("uuid")
                        .HasColumnName("student_id");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("uuid")
                        .HasColumnName("teacher_id");

                    b.HasKey("Id")
                        .HasName("attendance_pkey");

                    b.HasIndex("StudentId");

                    b.HasIndex("TeacherId");

                    b.ToTable("attendance", (string)null);
                });

            modelBuilder.Entity("SchoolManager.Models.AuditLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Action")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("action");

                    b.Property<string>("Details")
                        .HasColumnType("text")
                        .HasColumnName("details");

                    b.Property<string>("IpAddress")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("ip_address");

                    b.Property<string>("Resource")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("resource");

                    b.Property<Guid?>("SchoolId")
                        .HasColumnType("uuid")
                        .HasColumnName("school_id");

                    b.Property<DateTime?>("Timestamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("UserName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("user_name");

                    b.Property<string>("UserRole")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("user_role");

                    b.HasKey("Id")
                        .HasName("audit_logs_pkey");

                    b.HasIndex("SchoolId");

                    b.HasIndex("UserId");

                    b.ToTable("audit_logs", (string)null);
                });

            modelBuilder.Entity("SchoolManager.Models.DisciplineReport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("status")
                        .HasDefaultValueSql("'pendiente'::character varying");

                    b.Property<Guid?>("StudentId")
                        .HasColumnType("uuid")
                        .HasColumnName("student_id");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("uuid")
                        .HasColumnName("teacher_id");

                    b.HasKey("Id")
                        .HasName("discipline_reports_pkey");

                    b.HasIndex("StudentId");

                    b.HasIndex("TeacherId");

                    b.ToTable("discipline_reports", (string)null);
                });

            modelBuilder.Entity("SchoolManager.Models.Grade", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid?>("ActivityId")
                        .HasColumnType("uuid")
                        .HasColumnName("activity_id");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid?>("StudentId")
                        .HasColumnType("uuid")
                        .HasColumnName("student_id");

                    b.Property<decimal?>("Value")
                        .HasPrecision(3, 1)
                        .HasColumnType("numeric(3,1)")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("grades_pkey");

                    b.HasIndex("ActivityId");

                    b.HasIndex("StudentId");

                    b.ToTable("grades", (string)null);
                });

            modelBuilder.Entity("SchoolManager.Models.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Grade")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("grade");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("name");

                    b.Property<Guid?>("SchoolId")
                        .HasColumnType("uuid")
                        .HasColumnName("school_id");

                    b.HasKey("Id")
                        .HasName("groups_pkey");

                    b.HasIndex("SchoolId");

                    b.ToTable("groups", (string)null);
                });

            modelBuilder.Entity("SchoolManager.Models.School", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Address")
                        .HasColumnType("text")
                        .HasColumnName("address");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("LogoUrl")
                        .HasColumnType("text")
                        .HasColumnName("logo_url");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("phone");

                    b.HasKey("Id")
                        .HasName("schools_pkey");

                    b.ToTable("schools", (string)null);
                });

            modelBuilder.Entity("SchoolManager.Models.SecuritySetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int?>("ExpiryDays")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(90)
                        .HasColumnName("expiry_days");

                    b.Property<int?>("MaxLoginAttempts")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(5)
                        .HasColumnName("max_login_attempts");

                    b.Property<int?>("PasswordMinLength")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(8)
                        .HasColumnName("password_min_length");

                    b.Property<int?>("PreventReuse")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(5)
                        .HasColumnName("prevent_reuse");

                    b.Property<bool?>("RequireLowercase")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("require_lowercase");

                    b.Property<bool?>("RequireNumbers")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("require_numbers");

                    b.Property<bool?>("RequireSpecial")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("require_special");

                    b.Property<bool?>("RequireUppercase")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("require_uppercase");

                    b.Property<Guid?>("SchoolId")
                        .HasColumnType("uuid")
                        .HasColumnName("school_id");

                    b.Property<int?>("SessionTimeoutMinutes")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(30)
                        .HasColumnName("session_timeout_minutes");

                    b.HasKey("Id")
                        .HasName("security_settings_pkey");

                    b.HasIndex("SchoolId");

                    b.ToTable("security_settings", (string)null);
                });

            modelBuilder.Entity("SchoolManager.Models.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateOnly?>("BirthDate")
                        .HasColumnType("date")
                        .HasColumnName("birth_date");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Grade")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("grade");

                    b.Property<string>("GroupName")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("group_name");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_id");

                    b.Property<Guid?>("SchoolId")
                        .HasColumnType("uuid")
                        .HasColumnName("school_id");

                    b.HasKey("Id")
                        .HasName("students_pkey");

                    b.HasIndex("ParentId");

                    b.HasIndex("SchoolId");

                    b.ToTable("students", (string)null);
                });

            modelBuilder.Entity("SchoolManager.Models.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Code")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("code");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<Guid?>("SchoolId")
                        .HasColumnType("uuid")
                        .HasColumnName("school_id");

                    b.Property<bool?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("subjects_pkey");

                    b.HasIndex("SchoolId");

                    b.HasIndex(new[] { "Code" }, "subjects_code_key")
                        .IsUnique();

                    b.ToTable("subjects", (string)null);
                });

            modelBuilder.Entity("SchoolManager.Models.TeacherAssignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_id");

                    b.Property<Guid?>("SubjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("subject_id");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("uuid")
                        .HasColumnName("teacher_id");

                    b.HasKey("Id")
                        .HasName("teacher_assignments_pkey");

                    b.HasIndex("GroupId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("teacher_assignments", (string)null);
                });

            modelBuilder.Entity("SchoolManager.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("last_login");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("role");

                    b.Property<Guid?>("SchoolId")
                        .HasColumnType("uuid")
                        .HasColumnName("school_id");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("status")
                        .HasDefaultValueSql("'active'::character varying");

                    b.Property<bool?>("TwoFactorEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("two_factor_enabled");

                    b.HasKey("Id")
                        .HasName("users_pkey");

                    b.HasIndex("SchoolId");

                    b.HasIndex(new[] { "Email" }, "users_email_key")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("SchoolManager.Models.UserGroup", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("user_groups", (string)null);
                });

            modelBuilder.Entity("SchoolManager.Models.UserSubject", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "SubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("user_subjects", (string)null);
                });

            modelBuilder.Entity("SchoolManager.Models.Activity", b =>
                {
                    b.HasOne("SchoolManager.Models.Group", "Group")
                        .WithMany("Activities")
                        .HasForeignKey("GroupId")
                        .HasConstraintName("activities_group_id_fkey");

                    b.HasOne("SchoolManager.Models.School", "School")
                        .WithMany("Activities")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("activities_school_id_fkey");

                    b.HasOne("SchoolManager.Models.Subject", "Subject")
                        .WithMany("Activities")
                        .HasForeignKey("SubjectId")
                        .HasConstraintName("activities_subject_id_fkey");

                    b.HasOne("SchoolManager.Models.User", "Teacher")
                        .WithMany("Activities")
                        .HasForeignKey("TeacherId")
                        .HasConstraintName("activities_teacher_id_fkey");

                    b.Navigation("Group");

                    b.Navigation("School");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("SchoolManager.Models.Attendance", b =>
                {
                    b.HasOne("SchoolManager.Models.Student", "Student")
                        .WithMany("Attendances")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("attendance_student_id_fkey");

                    b.HasOne("SchoolManager.Models.User", "Teacher")
                        .WithMany("Attendances")
                        .HasForeignKey("TeacherId")
                        .HasConstraintName("attendance_teacher_id_fkey");

                    b.Navigation("Student");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("SchoolManager.Models.AuditLog", b =>
                {
                    b.HasOne("SchoolManager.Models.School", "School")
                        .WithMany("AuditLogs")
                        .HasForeignKey("SchoolId")
                        .HasConstraintName("audit_logs_school_id_fkey");

                    b.HasOne("SchoolManager.Models.User", "User")
                        .WithMany("AuditLogs")
                        .HasForeignKey("UserId")
                        .HasConstraintName("audit_logs_user_id_fkey");

                    b.Navigation("School");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SchoolManager.Models.DisciplineReport", b =>
                {
                    b.HasOne("SchoolManager.Models.Student", "Student")
                        .WithMany("DisciplineReports")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("discipline_reports_student_id_fkey");

                    b.HasOne("SchoolManager.Models.User", "Teacher")
                        .WithMany("DisciplineReports")
                        .HasForeignKey("TeacherId")
                        .HasConstraintName("discipline_reports_teacher_id_fkey");

                    b.Navigation("Student");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("SchoolManager.Models.Grade", b =>
                {
                    b.HasOne("SchoolManager.Models.Activity", "Activity")
                        .WithMany("Grades")
                        .HasForeignKey("ActivityId")
                        .HasConstraintName("grades_activity_id_fkey");

                    b.HasOne("SchoolManager.Models.Student", "Student")
                        .WithMany("Grades")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("grades_student_id_fkey");

                    b.Navigation("Activity");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("SchoolManager.Models.Group", b =>
                {
                    b.HasOne("SchoolManager.Models.School", "School")
                        .WithMany("Groups")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("groups_school_id_fkey");

                    b.Navigation("School");
                });

            modelBuilder.Entity("SchoolManager.Models.SecuritySetting", b =>
                {
                    b.HasOne("SchoolManager.Models.School", "School")
                        .WithMany("SecuritySettings")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("security_settings_school_id_fkey");

                    b.Navigation("School");
                });

            modelBuilder.Entity("SchoolManager.Models.Student", b =>
                {
                    b.HasOne("SchoolManager.Models.User", "Parent")
                        .WithMany("Students")
                        .HasForeignKey("ParentId")
                        .HasConstraintName("students_parent_id_fkey");

                    b.HasOne("SchoolManager.Models.School", "School")
                        .WithMany("Students")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("students_school_id_fkey");

                    b.Navigation("Parent");

                    b.Navigation("School");
                });

            modelBuilder.Entity("SchoolManager.Models.Subject", b =>
                {
                    b.HasOne("SchoolManager.Models.School", "School")
                        .WithMany("Subjects")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("subjects_school_id_fkey");

                    b.Navigation("School");
                });

            modelBuilder.Entity("SchoolManager.Models.TeacherAssignment", b =>
                {
                    b.HasOne("SchoolManager.Models.Group", "Group")
                        .WithMany("TeacherAssignments")
                        .HasForeignKey("GroupId")
                        .HasConstraintName("teacher_assignments_group_id_fkey");

                    b.HasOne("SchoolManager.Models.Subject", "Subject")
                        .WithMany("TeacherAssignments")
                        .HasForeignKey("SubjectId")
                        .HasConstraintName("teacher_assignments_subject_id_fkey");

                    b.HasOne("SchoolManager.Models.User", "Teacher")
                        .WithMany("TeacherAssignments")
                        .HasForeignKey("TeacherId")
                        .HasConstraintName("teacher_assignments_teacher_id_fkey");

                    b.Navigation("Group");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("SchoolManager.Models.User", b =>
                {
                    b.HasOne("SchoolManager.Models.School", "School")
                        .WithMany("Users")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("users_school_id_fkey");

                    b.Navigation("School");
                });

            modelBuilder.Entity("SchoolManager.Models.UserGroup", b =>
                {
                    b.HasOne("SchoolManager.Models.Group", "Group")
                        .WithMany("UserGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolManager.Models.User", "User")
                        .WithMany("UserGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SchoolManager.Models.UserSubject", b =>
                {
                    b.HasOne("SchoolManager.Models.Subject", "Subject")
                        .WithMany("UserSubjects")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolManager.Models.User", "User")
                        .WithMany("UserSubjects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SchoolManager.Models.Activity", b =>
                {
                    b.Navigation("Grades");
                });

            modelBuilder.Entity("SchoolManager.Models.Group", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("TeacherAssignments");

                    b.Navigation("UserGroups");
                });

            modelBuilder.Entity("SchoolManager.Models.School", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("AuditLogs");

                    b.Navigation("Groups");

                    b.Navigation("SecuritySettings");

                    b.Navigation("Students");

                    b.Navigation("Subjects");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("SchoolManager.Models.Student", b =>
                {
                    b.Navigation("Attendances");

                    b.Navigation("DisciplineReports");

                    b.Navigation("Grades");
                });

            modelBuilder.Entity("SchoolManager.Models.Subject", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("TeacherAssignments");

                    b.Navigation("UserSubjects");
                });

            modelBuilder.Entity("SchoolManager.Models.User", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("Attendances");

                    b.Navigation("AuditLogs");

                    b.Navigation("DisciplineReports");

                    b.Navigation("Students");

                    b.Navigation("TeacherAssignments");

                    b.Navigation("UserGroups");

                    b.Navigation("UserSubjects");
                });
#pragma warning restore 612, 618
        }
    }
}
