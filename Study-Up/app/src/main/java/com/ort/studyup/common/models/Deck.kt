package com.ort.studyup.common.models

class Deck(
    id: Int,
    name: String,
    difficulty: Int,
    subject: String,
    isHidden: Boolean,
    val flashcards: List<Any>,
) : DeckData(id, name, difficulty, subject, isHidden)

open class DeckData(
    val id: Int = -1,
    val name: String,
    val difficulty: Int,
    val subject: String,
    val isHidden: Boolean,
)

//TODO: add name tags
class NewDeckRequest(
    val name: String,
    val difficulty: Int,
    val subject: String,
    val isHidden: Boolean,
)