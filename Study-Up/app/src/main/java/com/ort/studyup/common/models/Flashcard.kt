package com.ort.studyup.common.models

import com.google.gson.annotations.SerializedName


class Flashcard(
        val id: Int,
        val question: String,
        val answer: String,
)

class NewFlashCardRequest(
        @SerializedName("deckId") val deckId: Int,
        @SerializedName("question") val question: String,
        @SerializedName("answer") val answer: String,
)

class EditFlashCardRequest(
        @SerializedName("question") val question: String,
        @SerializedName("answer") val answer: String,
)

class RateFlashCardRequest(
        @SerializedName("flashcardId") val id: Int,
        @SerializedName("score") val score: Int,
)

class RatedFlashcard(
        val id: Int,
        val question: String,
        val answer: String,
        var score: Int
)