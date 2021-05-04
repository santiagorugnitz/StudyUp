package com.ort.studyup.home.exams

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.renderers.DeckItemRenderer
import com.ort.studyup.common.renderers.ExamItemRenderer
import com.ort.studyup.common.renderers.SubtitleRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.repositories.DeckRepository
import com.ort.studyup.repositories.ExamRepository
import com.ort.studyup.repositories.UserRepository

class ExamsViewModel(
    private val examsRepository: ExamRepository,
    private val resourceWrapper: ResourceWrapper
) : BaseViewModel() {

    private val items = mutableListOf<Any>()

    fun loadExams(): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            items.clear()
            var exams = examsRepository.getExamList()
            exams = exams.sortedWith { a, b -> if (a.groupName.isNullOrEmpty()) -1 else 1 }

            items.add(
                SubtitleRenderer.Item(resourceWrapper.getString(R.string.unassigned))
            )
            var assigned = false
            exams.forEach {
                if (!assigned && !it.groupName.isNullOrEmpty()) {
                    SubtitleRenderer.Item(resourceWrapper.getString(R.string.assigned))
                    assigned = true
                }
                items.add(
                    ExamItemRenderer.Item(it.id, it.name, it.groupName)
                )
            }
            result.postValue(items)
        }
        return result
    }

}