package com.ort.studyup.home.exams

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.models.ExamItem
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
            val exams = examsRepository.exams()
            val groups = groupRepository.groups()

            val assigned = mutableListOf<ExamItem>()
            val unassigned = mutableListOf<ExamItem>()
            exams.forEach {
                if (it.groupName.isNullOrEmpty()) {
                    unassigned.add(it)
                } else {
                    assigned.add(it)
                }
            }

            val groupItems = groups.map { GroupItem(it.id, it.name) }
            if (unassigned.isNotEmpty()) {
                items.add(
                    SubtitleRenderer.Item(resourceWrapper.getString(R.string.unassigned_exams))
                )
                items.addAll(
                    unassigned.map { ExamItemRenderer.Item(it.id, it.name, it.groupName, groupItems) }
                )
            }
            if (assigned.isNotEmpty()) {
                items.add(
                    SubtitleRenderer.Item(resourceWrapper.getString(R.string.assigned_exams))
                )
                items.addAll(
                    assigned.map { ExamItemRenderer.Item(it.id, it.name, it.groupName, groupItems) }
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