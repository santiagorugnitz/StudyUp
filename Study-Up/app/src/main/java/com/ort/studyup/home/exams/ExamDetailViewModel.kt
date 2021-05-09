package com.ort.studyup.home.exams

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.models.Exam
import com.ort.studyup.common.models.Flashcard
import com.ort.studyup.common.renderers.ExamCardItemRenderer
import com.ort.studyup.common.renderers.ResultItemRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.DeckRepository
import com.ort.studyup.repositories.ExamRepository

class ExamDetailViewModel(
        private val examRepository: ExamRepository
) : BaseViewModel() {

    private val items = mutableListOf<Any>()

    fun loadCards(id: Int): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            items.clear()
            val exam = examRepository.getExam(id)
            items.addAll(
                    exam.examcards.map {
                        ExamCardItemRenderer.Item(
                                it.id,
                                it.question,
                                it.answer
                        )
                    }
            )
            result.postValue(items)
        }
        return result
    }

    fun loadResults(id: Int): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            items.clear()
            val response = examRepository.results(id)
            items.addAll(
                    response.mapIndexed { pos, it ->
                        ResultItemRenderer.Item(
                                pos,
                                it.username,
                                it.score
                        )
                    }
            )
            result.postValue(items)
        }
        return result
    }

}