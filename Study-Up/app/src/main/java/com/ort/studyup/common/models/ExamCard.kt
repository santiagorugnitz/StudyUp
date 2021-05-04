package com.ort.studyup.common.models

import com.google.gson.annotations.SerializedName


class ExamCard(
        val id: Int,
        val question: String,
        val answer: Boolean,
)

class NewExamCardRequest(
        @SerializedName("examId") val examId: Int,
        @SerializedName("question") val question: String,
        @SerializedName("answer") val answer: Boolean,
)

class EditExamCardRequest(
        @SerializedName("question") val question: String,
        @SerializedName("answer") val answer: Boolean,
)

