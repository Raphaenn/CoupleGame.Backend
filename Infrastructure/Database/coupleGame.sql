CREATE TABLE "answers"(
    "id" UUID NOT NULL,
    "user_id" UUID NOT NULL,
    "answer_1" VARCHAR(255) NOT NULL,
    "answer_2" VARCHAR(255) NOT NULL,
    "answer_3" VARCHAR(255) NOT NULL,
    "answer_4" VARCHAR(255) NOT NULL,
    "answer_5" VARCHAR(255) NOT NULL,
    "answer_6" VARCHAR(255) NOT NULL,
    "created_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL,
    "quiz_id" UUID NOT NULL
);
ALTER TABLE
    "answers" ADD PRIMARY KEY("id");
CREATE TABLE "topic"(
    "id" UUID NOT NULL,
    "name" VARCHAR(255) NOT NULL,
    "description" VARCHAR(255) NOT NULL,
    "status" BOOLEAN NOT NULL
);
ALTER TABLE
    "topic" ADD PRIMARY KEY("id");
ALTER TABLE
    "topic" ADD CONSTRAINT "topic_name_unique" UNIQUE("name");
CREATE TABLE "users"(
    "id" UUID NOT NULL,
    "name" VARCHAR(255) NOT NULL,
    "email" VARCHAR(255) NOT NULL,
    "password" INTEGER NOT NULL,
    "birthdate" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL
);
ALTER TABLE
    "users" ADD PRIMARY KEY("id");
ALTER TABLE
    "users" ADD CONSTRAINT "user_email_unique" UNIQUE("email");
CREATE TABLE "quiz"(
    "id" UUID NOT NULL,
    "creted_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL,
    "question_id_1" UUID NOT NULL,
    "question_id_2" UUID NOT NULL,
    "question_id_3" UUID NOT NULL,
    "question_id_4" UUID NOT NULL,
    "question_id_5" UUID NOT NULL,
    "question_id_6" UUID NOT NULL,
    "couple_id" UUID NOT NULL
);
ALTER TABLE
    "quiz" ADD PRIMARY KEY("id");
ALTER TABLE
    "quiz" ADD CONSTRAINT "quiz_question_id_1_unique" UNIQUE("question_id_1");
ALTER TABLE
    "quiz" ADD CONSTRAINT "quiz_question_id_2_unique" UNIQUE("question_id_2");
ALTER TABLE
    "quiz" ADD CONSTRAINT "quiz_question_id_3_unique" UNIQUE("question_id_3");
ALTER TABLE
    "quiz" ADD CONSTRAINT "quiz_question_id_4_unique" UNIQUE("question_id_4");
ALTER TABLE
    "quiz" ADD CONSTRAINT "quiz_question_id_5_unique" UNIQUE("question_id_5");
ALTER TABLE
    "quiz" ADD CONSTRAINT "quiz_question_id_6_unique" UNIQUE("question_id_6");
ALTER TABLE
    "quiz" ADD CONSTRAINT "quiz_couple_id_unique" UNIQUE("couple_id");
CREATE TABLE "login"(
    "id" UUID NOT NULL,
    "token" VARCHAR(255) NOT NULL,
    "user_id" UUID NOT NULL,
    "expire_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL
);
ALTER TABLE
    "login" ADD PRIMARY KEY("id");
ALTER TABLE
    "login" ADD CONSTRAINT "login_user_id_unique" UNIQUE("user_id");
CREATE TABLE "couple"(
    "id" UUID NOT NULL,
    "couple_one" UUID NOT NULL,
    "couple_two" UUID NOT NULL,
    "type" VARCHAR(255) NULL,
    "status" VARCHAR(255) NOT NULL,
    "created_at" TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL
);
ALTER TABLE
    "couple" ADD PRIMARY KEY("id");
CREATE TABLE "questions"(
    "id" UUID NOT NULL,
    "question" VARCHAR(255) NOT NULL,
    "answer_1" VARCHAR(255) NOT NULL,
    "answer_2" VARCHAR(255) NOT NULL,
    "answer_3" VARCHAR(255) NOT NULL,
    "answer_4" VARCHAR(255) NOT NULL,
    "topic_id" UUID NOT NULL
);
ALTER TABLE
    "questions" ADD PRIMARY KEY("id");
ALTER TABLE
    "questions" ADD CONSTRAINT "questions_question_unique" UNIQUE("question");
ALTER TABLE
    "answers" ADD CONSTRAINT "answers_quiz_id_foreign" FOREIGN KEY("quiz_id") REFERENCES "quiz"("id");
ALTER TABLE
    "couple" ADD CONSTRAINT "couple_couple_two_foreign" FOREIGN KEY("couple_two") REFERENCES "users"("id");
ALTER TABLE
    "quiz" ADD CONSTRAINT "quiz_question_id_1_foreign" FOREIGN KEY("question_id_1") REFERENCES "questions"("id");
ALTER TABLE
    "quiz" ADD CONSTRAINT "quiz_question_id_6_foreign" FOREIGN KEY("question_id_6") REFERENCES "questions"("id");
ALTER TABLE
    "quiz" ADD CONSTRAINT "quiz_question_id_4_foreign" FOREIGN KEY("question_id_4") REFERENCES "questions"("id");
ALTER TABLE
    "login" ADD CONSTRAINT "login_user_id_foreign" FOREIGN KEY("user_id") REFERENCES "users"("id");
ALTER TABLE
    "couple" ADD CONSTRAINT "couple_couple_one_foreign" FOREIGN KEY("couple_one") REFERENCES "users"("id");
ALTER TABLE
    "answers" ADD CONSTRAINT "answers_user_id_foreign" FOREIGN KEY("user_id") REFERENCES "users"("id");
ALTER TABLE
    "quiz" ADD CONSTRAINT "quiz_question_id_5_foreign" FOREIGN KEY("question_id_5") REFERENCES "questions"("id");
ALTER TABLE
    "quiz" ADD CONSTRAINT "quiz_question_id_3_foreign" FOREIGN KEY("question_id_3") REFERENCES "questions"("id");
ALTER TABLE
    "quiz" ADD CONSTRAINT "quiz_couple_id_foreign" FOREIGN KEY("couple_id") REFERENCES "couple"("id");
ALTER TABLE
    "questions" ADD CONSTRAINT "questions_topic_id_foreign" FOREIGN KEY("topic_id") REFERENCES "topic"("id");
ALTER TABLE
    "quiz" ADD CONSTRAINT "quiz_question_id_2_foreign" FOREIGN KEY("question_id_2") REFERENCES "questions"("id");