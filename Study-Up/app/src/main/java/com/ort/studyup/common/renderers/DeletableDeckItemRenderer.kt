package com.ort.studyup.common.renderers

import android.view.View
import android.view.ViewGroup
import com.ort.studyup.R
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.item_deletable_deck.view.*

class DeletableDeckItemRenderer(private val callback: Callback) : ViewRenderer<DeletableDeckItemRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.item_deletable_deck, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        view.deckName.text = model.name
        view.deleteButton.setOnClickListener {
            callback.onDeleteDeck(model.groupId, model.id)
        }
    }

    class Item(
        val id: Int,
        val name: String,
        val groupId: Int,
    )

    interface Callback {
        fun onDeleteDeck(groupId: Int, deckId: Int)
    }
}