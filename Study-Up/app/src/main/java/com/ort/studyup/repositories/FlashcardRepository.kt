package com.ort.studyup.repositories

import com.ort.studyup.common.models.*
import com.ort.studyup.services.FlashcardService
import com.ort.studyup.services.check

class FlashcardRepository(
    private val flashcardService: FlashcardService,
) {

    suspend fun createFlashcard(deckId: Int, question: String, answer: String): Flashcard {
        return flashcardService.createFlashcard(
            NewFlashCardRequest(deckId, question, answer)
        ).check()
    }

    suspend fun updateFlashcard(id: Int, question: String, answer: String) {
        flashcardService.updateFlashcard(
            id,
            EditFlashCardRequest(question, answer)
        ).check()
    }

    suspend fun deleteFlashcard(id: Int) {
        flashcardService.deleteFlashcard(id).check()
    }

    suspend fun ratedFlashcards(deckId: Int): List<RatedFlashcard> {
        return flashcardService.ratedFlashcards(deckId).check()
    }

    suspend fun updateScore(flashcards: List<RatedFlashcard>) {
        flashcardService.updateScore(
            flashcards.map {
                RateFlashCardRequest(
                    it.id,
                    it.score
                )
            }
        ).check()
    }

    suspend fun comment(id: Int, comment: String) {
        flashcardService.comment(id, CommentRequest(comment)).check()
    }

}