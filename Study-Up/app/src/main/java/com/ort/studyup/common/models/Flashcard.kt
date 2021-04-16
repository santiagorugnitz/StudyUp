package com.ort.studyup.common.models


class Flashcard(
        val id: Int,
        val question: String,
        val answer: String,
)

//TODO: add name tags
class NewFlashCardRequest(
        val deckId: Int,
        val question: String,
        val answer: String,
)

class EditFlashCardRequest(
        val question: String,
        val answer: String,
)
