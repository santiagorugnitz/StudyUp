package com.ort.studyup.repositories

import com.ort.studyup.common.models.EditFlashCardRequest
import com.ort.studyup.common.models.Flashcard
import com.ort.studyup.common.models.NewFlashCardRequest
import com.ort.studyup.common.models.RatedFlashcard
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

    suspend fun updateScore(flashcards:List<RatedFlashcard>){
        //flashcardService.updateScore().check()
    }

}