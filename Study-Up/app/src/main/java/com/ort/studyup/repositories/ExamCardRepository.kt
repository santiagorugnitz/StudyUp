package com.ort.studyup.repositories

import com.ort.studyup.common.models.EditExamCardRequest
import com.ort.studyup.common.models.ExamCard
import com.ort.studyup.common.models.NewExamCardRequest
import com.ort.studyup.services.ExamCardService
import com.ort.studyup.services.check

class ExamCardRepository(
    private val examCardService: ExamCardService,
) {

    suspend fun createExamCard(examId: Int, question: String, answer: Boolean): ExamCard {
        return examCardService.createExam(
            NewExamCardRequest(examId, question, answer)
        ).check()
    }

    suspend fun updateExamCard(id: Int, question: String, answer: Boolean) {
        examCardService.editExamCard(
            id,
            EditExamCardRequest(question, answer)
        ).check()
    }

    suspend fun deleteExamCard(id: Int) {
        examCardService.deleteExamCard(id).check()
    }

}