package com.ort.studyup.common.models

import com.google.gson.annotations.SerializedName


class Group(
    val id: Int,
    val name: String,
    val decks: List<DeckItem>
)

class NewGroupRequest(
    @SerializedName("name") val name: String,
)

class GroupSearchResponse(
    @SerializedName("id") val id: Int,
    @SerializedName("name") val name: String,
    @SerializedName("subscribed") val subscribed: Boolean,
    @SerializedName("teachersName") val teacherName: String,
)


class DeckItem(
    val id: Int,
    val name: String
) {
    override fun equals(other: Any?): Boolean {
        return if (other is DeckItem) {
            other.id == id
        } else {
            false
        }
    }
}

class GroupItem(
    val id: Int,
    val name: String
) {
    override fun equals(other: Any?): Boolean {
        return if (other is DeckItem) {
            other.id == id
        } else {
            false
        }
    }
}

class TaskResponse(
    val decks: List<DeckItem>,
    val exams: List<ExamItem>
)
