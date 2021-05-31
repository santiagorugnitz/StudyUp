package com.ort.studyup.home.exams

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.repositories.ExamRepository
import com.ort.studyup.services.ServiceError

class NewExamViewModel(
    private val resourceWrapper: ResourceWrapper,
    private val examRepository: ExamRepository
) : BaseViewModel() {


    fun sendData(name: String, subject: String, difficulty: Int): LiveData<Int> {
        val result = MutableLiveData<Int>()
        executeService {
            if (name.isNotEmpty() && subject.isNotEmpty()) {
                result.postValue(examRepository.createExam(name, subject, difficulty).id)
            } else {
                error.postValue(ServiceError(INTERNAL_ERROR_CODE, resourceWrapper.getString(R.string.error_empty_fields)))
                result.postValue(-1)
            }
        }
        return result
    }

}