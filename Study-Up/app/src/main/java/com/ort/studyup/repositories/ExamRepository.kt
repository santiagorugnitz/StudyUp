package com.ort.studyup.repositories

import com.ort.studyup.common.models.*
import com.ort.studyup.services.DeckService
import com.ort.studyup.services.ExamService
import com.ort.studyup.services.check

class ExamRepository(
    private val examService: ExamService,
) {

    suspend fun getExamList(): List<ExamItem> {
        return examService.exams().check()
    }

    suspend fun createExam(name: String, subject: String, difficulty: Int): ExamItem {
        return examService.createExam(
            NewExamRequest(name, subject, difficulty)
        ).check()
    }

    suspend fun getExam(id: Int): Exam {
        return examService.getExam(id).check()
    }

    suspend fun assignToGroup(examId: Int, groupId: Int) {
        examService.assignToGroup(examId, groupId).check()
    }


}