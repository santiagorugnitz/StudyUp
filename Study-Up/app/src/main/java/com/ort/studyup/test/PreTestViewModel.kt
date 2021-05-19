package com.ort.studyup.test

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.*
import com.ort.studyup.common.renderers.ResultItemRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.ExamCardRepository
import com.ort.studyup.repositories.ExamRepository
import com.ort.studyup.repositories.FlashcardRepository
import com.ort.studyup.repositories.UserRepository

class PreTestViewModel(
    private val examRepository: ExamRepository,
    private val userRepository: UserRepository
) : BaseViewModel() {

    var examResult: Double? = null
    var user: User? = null

    fun loadExam(id: Int): LiveData<Pair<Exam, List<Any>>> {
        val result = MutableLiveData<Pair<Exam, List<Any>>>()
        executeService {
            user = userRepository.getUser()
            val exam = examRepository.getExam(id)
            val results = examRepository.results(id)
            examResult = results.find { it.username == user?.username }?.score
            result.postValue(Pair(exam, results.mapIndexed { pos, it ->
                ResultItemRenderer.Item(
                    pos + 1,
                    it.username,
                    it.score
                )
            }))
        }
        return result
    }

    fun reloadResults(id: Int): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            val results = examRepository.results(id)
            examResult = results.find { it.username == user?.username }?.score
            result.postValue(results.mapIndexed { pos, it ->
                ResultItemRenderer.Item(
                    pos + 1,
                    it.username,
                    it.score
                )
            })
        }
        return result

    }

}