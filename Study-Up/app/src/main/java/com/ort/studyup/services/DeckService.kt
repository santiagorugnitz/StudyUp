package com.ort.studyup.services

import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.models.NewDeckRequest
import retrofit2.Response
import retrofit2.http.*

interface DeckService {

    @GET("api/decks")
    suspend fun decksFromUser(@Query("userId") userId: Int): Response<List<Deck>>

    @POST("api/decks")
    suspend fun createDeck(@Body data: NewDeckRequest): Response<DeckData>

    @PUT("api/decks/{id}")
    suspend fun updateDeck(@Path("id") id: Int, @Body data: NewDeckRequest): Response<*>

    @DELETE("api/decks/{id}")
    suspend fun deleteDeck(@Path("id") id: Int): Response<*>

    @GET("api/decks/{id}")
    suspend fun getDeck(@Path("id") id: Int): Response<Deck>

    @GET("api/decks/following")
    suspend fun getFollowingDecks(): Response<List<Deck>>

}