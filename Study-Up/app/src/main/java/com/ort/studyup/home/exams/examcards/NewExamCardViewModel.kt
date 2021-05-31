package com.ort.studyup.home.exams.examcards

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.repositories.ExamCardRepository
import com.ort.studyup.services.ServiceError

class NewExamCardViewModel(
    private val resourceWrapper: ResourceWrapper,
    private val examCardRepository: ExamCardRepository
) : BaseViewModel() {

    var examCardId = -1

    fun sendData(deckId: Int, question: String, answer: Boolean): LiveData<Int> {
        val result = MutableLiveData<Int>()
        executeService {
            if (question.isNotEmpty()) {
                if (examCardId != -1) {
                    examCardRepository.updateExamCard(examCardId, question, answer)
                } else {
                    examCardId = examCardRepository.createExamCard(deckId, question, answer).id
                }
                result.postValue(examCardId)
            } else {
                error.postValue(ServiceError(INTERNAL_ERROR_CODE, resourceWrapper.getString(R.string.error_empty_fields)))
                result.postValue(-1)
            }
        }
        return result
    }

    fun deleteExamCard(): LiveData<Boolean> {
        val result = MutableLiveData<Boolean>()
        executeService {
            examCardRepository.deleteExamCard(examCardId)
            result.postValue(true)
        }
        return result
    }

}