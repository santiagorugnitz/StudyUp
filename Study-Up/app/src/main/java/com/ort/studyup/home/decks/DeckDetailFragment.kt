package com.ort.studyup.home.decks

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.observe
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.ort.studyup.R
import com.ort.studyup.common.DECK_DATA_KEY
import com.ort.studyup.common.DECK_ID_KEY
import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.renderers.FlashcardItemRenderer
import com.ort.studyup.common.ui.BaseFragment
import com.thinkup.easylist.RendererAdapter
import kotlinx.android.synthetic.main.fragment_deck_detail.*
import kotlinx.android.synthetic.main.fragment_decks.title

class DeckDetailFragment : BaseFragment(), FlashcardItemRenderer.Callback {

    private val adapter = RendererAdapter()
    private val viewModel: DeckDetailViewModel by injectViewModel(DeckDetailViewModel::class)

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_deck_detail, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        val deckId = arguments?.getInt(DECK_ID_KEY)

        adapter.addRenderer(FlashcardItemRenderer(this))
        flashcardList.layoutManager = LinearLayoutManager(requireContext())
        flashcardList.adapter = adapter
        initViewModel(deckId ?: 0)
    }

    private fun initUI(deck: Deck) {
        title.text = deck.name
        subject.text = deck.subject
        difficulty.text = resources.getStringArray(R.array.difficulties)[deck.difficulty]
        visibility.text = resources.getStringArray(R.array.visibilities)[if (deck.isHidden) 1 else 0]
        creator.text = deck.creator

        editLink.setOnClickListener {
            findNavController().navigate(R.id.action_deckDetailFragment_to_newDeckFragment, Bundle().apply { putSerializable(DECK_DATA_KEY, deck as DeckData) })
        }
        addButton.setOnClickListener {
            findNavController().navigate(R.id.action_deckDetailFragment_to_newFlashcardFragment, Bundle().apply { putInt(DECK_ID_KEY,deck.id) })
        }
    }

    private fun initViewModel(id: Int) {
        viewModel.loadDetails(id).observe(viewLifecycleOwner, { deck ->
            adapter.setItems(deck.flashcards.map{
                FlashcardItemRenderer.Item(
                        it.id,
                        it.question,
                        it.answer
                )
            })
            initUI(deck)
        }
        )
    }

    override fun onEditFlashcard(id: Int, question: String, answer: String) {
        //TODO navigate to NewFlashcardFragment with values on bundle
    }

    override fun onShowAnswerChanged() {
        adapter.notifyDataSetChanged()
    }

}