package com.ort.studyup.common.models

import com.google.gson.annotations.SerializedName
import java.io.Serializable

class Exam(
    val id: Int,
    val name: String,
    val difficulty: Int,
    val subject: String,
    val examcards: List<ExamCard>,
    val groupsName: String
)

class ExamItem(
    @SerializedName("id") val id: Int,
    @SerializedName("name") val name: String,
    @SerializedName("groupsName") val groupName: String?,
)

class NewExamRequest(
    val name: String,
    val subject: String,
    val difficulty: Int
)
