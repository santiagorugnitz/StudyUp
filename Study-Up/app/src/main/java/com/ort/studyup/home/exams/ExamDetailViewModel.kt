package com.ort.studyup.home.exams

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.models.Exam
import com.ort.studyup.common.models.Flashcard
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.DeckRepository
import com.ort.studyup.repositories.ExamRepository

class ExamDetailViewModel(
    private val examRepository: ExamRepository
) : BaseViewModel() {

    private val items = mutableListOf<Any>()

    fun loadDetails(id: Int): LiveData<Exam> {
        val result = MutableLiveData<Exam>()
        executeService {
            items.clear()
            result.postValue(examRepository.getExam(id))
        }
        return result
    }

}