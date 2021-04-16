package com.ort.studyup.common.models

import java.io.Serializable

class Deck(
        id: Int,
        creator: String,
        name: String,
        difficulty: Int,
        subject: String,
        isHidden: Boolean,
        val flashcards: List<Flashcard>,
) : DeckData(id, creator, name, difficulty, subject, isHidden)

open class DeckData(
        val id: Int = -1,
        val creator: String,
        val name: String,
        val difficulty: Int,
        val subject: String,
        val isHidden: Boolean,
) : Serializable

//TODO: add name tags
class NewDeckRequest(
        val name: String,
        val difficulty: Int,
        val subject: String,
        val isHidden: Boolean,
)

