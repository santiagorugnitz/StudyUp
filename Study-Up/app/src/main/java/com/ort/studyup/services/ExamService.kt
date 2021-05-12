package com.ort.studyup.services

import com.ort.studyup.common.models.*
import retrofit2.Response
import retrofit2.http.*

interface ExamService {

    @GET("api/exams")
    suspend fun exams(): Response<List<ExamItem>>

    @POST("api/exams")
    suspend fun createExam(@Body data: NewExamRequest): Response<ExamItem>

    @GET("api/exams/{id}")
    suspend fun getExam(@Path("id") id: Int): Response<Exam>

    @POST("api/exams/{id}/assign")
    suspend fun assignToGroup(@Path("id") id: Int, @Query("groupId") groupId: Int): Response<*>

    @GET("api/exams/{id}/results")
    suspend fun results(@Path("id") id: Int): Response<List<ExamResult>>

    @POST("api/exams/{id}/results")
    suspend fun sendResults(@Path("id") id: Int, @Body() data: NewResultRequest): Response<Unit>
}