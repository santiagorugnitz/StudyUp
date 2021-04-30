package com.ort.studyup.common.renderers

import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import android.widget.ArrayAdapter
import android.widget.Spinner
import com.ort.studyup.R
import com.ort.studyup.common.models.DeckItem
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.item_assign_deck.view.*

class AssignDeckItemRenderer(private val callback: Callback) : ViewRenderer<AssignDeckItemRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.item_assign_deck, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        view.name.text = model.name
        val spinner = Spinner(view.context, Spinner.MODE_DIALOG)
        view.assignButton.setOnClickListener {
            spinner.performClick()
        }
        ArrayAdapter(view.context, android.R.layout.simple_spinner_item, model.decks.map { it.name }.toTypedArray()).also {
            it.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
            spinner.adapter = it
        }
        spinner.onItemSelectedListener = object : AdapterView.OnItemSelectedListener {
            override fun onItemSelected(p0: AdapterView<*>?, p1: View?, position: Int, p3: Long) {
                callback.onAssignDeck(model.id, model.decks[position].id)
            }

            override fun onNothingSelected(p0: AdapterView<*>?) {
            }

        }
    }

    class Item(
        val id: Int,
        val name: String,
        val decks: List<DeckItem>,
    )

    interface Callback {
        fun onAssignDeck(groupId: Int, deckId: Int)
    }
}