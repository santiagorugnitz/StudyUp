package com.ort.studyup.repositories

import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.models.NewDeckRequest
import com.ort.studyup.services.DeckService
import com.ort.studyup.services.check

class DeckRepository(
    private val deckService: DeckService,
) {

    suspend fun decksFromUser(id: Int): List<DeckData> {
        return deckService.decksFromUser(id).check()
    }

    suspend fun createDeck(deckData: DeckData): DeckData {
        return deckService.createDeck(
            NewDeckRequest(deckData.name, deckData.difficulty, deckData.subject, deckData.isHidden)
        ).check()
    }

    suspend fun updateDeck(id: Int, deckData: DeckData) {
        deckService.updateDeck(
            id,
            NewDeckRequest(deckData.name, deckData.difficulty, deckData.subject, deckData.isHidden)
        ).check()
    }

    suspend fun deleteDeck(id: Int) {
        deckService.deleteDeck(id).check()
    }

    suspend fun getDeck(id: Int): Deck {
        return deckService.getDeck(id).check()
    }

    suspend fun getFollowingDecks(): List<Deck> {
        return deckService.getFollowingDecks().check()
    }


}