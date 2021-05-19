package com.ort.studyup.test

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.ExamCard
import com.ort.studyup.common.models.RatedFlashcard
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.ExamCardRepository
import com.ort.studyup.repositories.ExamRepository
import com.ort.studyup.repositories.FlashcardRepository

class TestViewModel(
        private val examRepository: ExamRepository
) : BaseViewModel() {

    private var examCards = listOf<ExamCard>()
    private var currentPos = 0
    var correctAnswers = 0
    private var examId = 0

    fun loadCards(id: Int): LiveData<ExamCard> {
        examId = id
        val result = MutableLiveData<ExamCard>()
        executeService {
            examCards = examRepository.getExam(examId).examcards
            result.postValue(examCards.firstOrNull())
        }
        return result
    }

    fun getTotal() = examCards.size

    fun onAnswer(answer: Boolean): LiveData<ExamCard> {
        val result = MutableLiveData<ExamCard>()
        executeService {
            if (examCards[currentPos].answer == answer) {
                correctAnswers++
            }
            currentPos++
            result.postValue(examCards.getOrNull(currentPos))
        }
        return result
    }


    fun sendAnswers(time: Int) {
        executeService {
            examRepository.sendResults(examId, time, correctAnswers)
        }
    }
}