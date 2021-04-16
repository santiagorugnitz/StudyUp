package com.ort.studyup.repositories

import com.ort.studyup.common.models.*
import com.ort.studyup.services.DeckService
import com.ort.studyup.services.FlashcardService
import com.ort.studyup.services.check

class FlashcardRepository(
        private val flashcardService: FlashcardService,
) {

    suspend fun createFlashcard(deckId: Int,question: String, answer: String){
        flashcardService.createFlashcard(
            NewFlashCardRequest(deckId,question,answer)
        ).check()
    }

    suspend fun editFlashcard(id: Int,question: String, answer: String){
        flashcardService.updateFlashcard(
            id,
            EditFlashCardRequest(question, answer)
        ).check()
    }

    suspend fun deleteDeck(id:Int){
        flashcardService.deleteFlashcard(id).check()
    }

}