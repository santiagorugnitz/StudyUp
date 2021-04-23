package com.ort.studyup.common.models

import com.google.gson.annotations.SerializedName
import java.io.Serializable

class Deck(
        id: Int,
        author: String,
        name: String,
        difficulty: Int,
        subject: String,
        isHidden: Boolean,
        val flashcards: List<Flashcard>,
) : DeckData(id, author, name, difficulty, subject, isHidden)

open class DeckData(
        val id: Int = -1,
        val author: String,
        val name: String,
        val difficulty: Int,
        val subject: String,
        val isHidden: Boolean,
) : Serializable

class NewDeckRequest(
        @SerializedName("name") val name: String,
        @SerializedName("difficulty") val difficulty: Int,
        @SerializedName("subject") val subject: String,
        @SerializedName("isHidden") val isHidden: Boolean,
)

