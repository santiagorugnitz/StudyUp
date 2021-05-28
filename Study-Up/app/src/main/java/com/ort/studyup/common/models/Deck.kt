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
    @SerializedName("id")val id: Int = -1,
    @SerializedName("author")val author: String,
    @SerializedName("name")val name: String,
    @SerializedName("difficulty")val difficulty: Int,
    @SerializedName("subject")val subject: String,
    @SerializedName("isHidden")val isHidden: Boolean,
    @SerializedName("flashcardCount")val flashcardCount: Int? = null,
) : Serializable

class NewDeckRequest(
    @SerializedName("name") val name: String,
    @SerializedName("difficulty") val difficulty: Int,
    @SerializedName("subject") val subject: String,
    @SerializedName("isHidden") val isHidden: Boolean,
)

