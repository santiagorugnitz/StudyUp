package com.ort.studyup.home.exams

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.models.GroupItem
import com.ort.studyup.common.renderers.ExamItemRenderer
import com.ort.studyup.common.renderers.SubtitleRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.repositories.ExamRepository
import com.ort.studyup.repositories.GroupRepository

class ExamsViewModel(
    private val examsRepository: ExamRepository,
    private val groupRepository: GroupRepository,
    private val resourceWrapper: ResourceWrapper
) : BaseViewModel() {

    private val items = mutableListOf<Any>()

    fun loadExams(): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            items.clear()
            var exams = examsRepository.exams()
            val groups = groupRepository.groups()
            exams = exams.sortedWith { a, b -> if (a.groupName.isNullOrEmpty()) -1 else 1 }

            items.add(
                SubtitleRenderer.Item(resourceWrapper.getString(R.string.unassigned))
            )
            var assigned = false
            exams.forEach { examItem ->
                if (!assigned && !examItem.groupName.isNullOrEmpty()) {
                    SubtitleRenderer.Item(resourceWrapper.getString(R.string.assigned))
                    assigned = true
                }
                items.add(
                    ExamItemRenderer.Item(examItem.id, examItem.name, examItem.groupName, groups.map { GroupItem(it.id, it.name) })
                )
            }
            result.postValue(items)
        }
        return result
    }

    fun onAssignExam(examId: Int, groupId: Int): LiveData<Boolean> {
        val result = MutableLiveData<Boolean>()
        executeService {
            examsRepository.assignToGroup(examId, groupId)
            result.postValue(true)
        }
        return result
    }

}