package com.ort.studyup.study

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.RatedFlashcard
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.FlashcardRepository

class StudyViewModel(
    private val flashcardRepository: FlashcardRepository
) : BaseViewModel() {

    private var flashcards = listOf<RatedFlashcard>()
    private var currentPos = 0

    fun loadFlashcards(deckId: Int): LiveData<RatedFlashcard> {
        val result = MutableLiveData<RatedFlashcard>()
        executeService {
            flashcards = flashcardRepository.ratedFlashcards(deckId)
            flashcards = flashcards.sortedBy { it.score }
            result.postValue(flashcards.firstOrNull())
        }
        return result
    }

    fun onWrong(): LiveData<RatedFlashcard> {
        val result = MutableLiveData<RatedFlashcard>()
        executeService {
            val previousId = flashcards[currentPos].id
            flashcards[currentPos].score--
            flashcards = flashcards.sortedBy { it.score }
            currentPos = (currentPos + 1) % flashcards.size
            if (flashcards[currentPos].id == previousId) {
                currentPos = (currentPos + 1) % flashcards.size
            }
            result.postValue(flashcards[currentPos])
        }
        return result
    }

    fun onCorrect(): LiveData<RatedFlashcard> {
        val result = MutableLiveData<RatedFlashcard>()
        executeService {
            val previousId = flashcards[currentPos].id
            flashcards[currentPos].score++
            flashcards = flashcards.sortedBy { it.score }
            currentPos = (currentPos + 1) % flashcards.size
            if (flashcards[currentPos].id == previousId) {
                currentPos = (currentPos + 1) % flashcards.size
            }
            result.postValue(flashcards[currentPos])
        }
        return result
    }

    fun updateScore() {
        if (flashcards.isEmpty()) return
        executeService {
            flashcardRepository.updateScore(flashcards)
        }
    }
}