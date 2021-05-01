package com.ort.studyup.common.renderers

import android.view.View
import android.view.ViewGroup
import com.ort.studyup.R
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.item_deck.view.*

class DeckItemRenderer(private val callback: Callback) : ViewRenderer<DeckItemRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.item_deck, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        view.deckName.text = model.name
        view.setOnClickListener {
            callback.onDeckClicked(model.id)
        }
    }

    class Item(
        val id:Int,
        val name: String
    )

    interface Callback {
        fun onDeckClicked(deckId: Int)
    }
}