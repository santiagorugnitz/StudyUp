package com.ort.studyup.home.decks.flashcards

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.repositories.FlashcardRepository
import com.ort.studyup.services.ServiceError

class NewFlashcardViewModel(
    private val resourceWrapper: ResourceWrapper,
    private val flashcardRepository: FlashcardRepository
) : BaseViewModel() {

    var flashcardId = -1

    fun sendData(deckId: Int, question: String, answer: String): LiveData<Int> {
        val result = MutableLiveData<Int>()
        executeService {
            if (validate(question) && validate(answer)) {
                if (flashcardId != -1) {
                    flashcardRepository.updateFlashcard(flashcardId, question, answer)
                } else {
                    flashcardId = flashcardRepository.createFlashcard(deckId, question, answer).id
                }
                result.postValue(flashcardId)
            } else {
                error.postValue(ServiceError(INTERNAL_ERROR_CODE, resourceWrapper.getString(R.string.error_empty_fields)))
                result.postValue(-1)
            }
        }
        return result
    }

    private fun validate(field: String): Boolean {
        return field.isNotEmpty()
    }

    fun deleteFlashcard(): LiveData<Boolean> {
        val result = MutableLiveData<Boolean>()
        executeService {
            flashcardRepository.deleteFlashcard(flashcardId)
            result.postValue(true)
        }
        return result
    }

}