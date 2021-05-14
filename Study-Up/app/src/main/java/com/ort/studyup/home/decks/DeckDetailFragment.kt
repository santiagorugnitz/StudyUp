package com.ort.studyup.home.decks

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.observe
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import com.ort.studyup.R
import com.ort.studyup.common.*
import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.renderers.FlashcardItemRenderer
import com.ort.studyup.common.ui.BaseFragment
import com.ort.studyup.study.StudyActivity
import com.thinkup.easylist.RendererAdapter
import kotlinx.android.synthetic.main.fragment_deck_detail.*

class DeckDetailFragment : BaseFragment(), FlashcardItemRenderer.Callback {

    private val adapter = RendererAdapter()
    private val viewModel: DeckDetailViewModel by injectViewModel(DeckDetailViewModel::class)
    private var deckId: Int = 0

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_deck_detail, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        deckId = arguments?.getInt(DECK_ID_KEY) ?: 0

        adapter.addRenderer(FlashcardItemRenderer(this))
        flashcardList.layoutManager = LinearLayoutManager(requireContext())
        flashcardList.adapter = adapter
        initViewModel(deckId)
    }

    private fun initUI(deck: Deck) {
        title.text = deck.name
        subject.text = deck.subject
        difficulty.text = resources.getStringArray(R.array.difficulties)[deck.difficulty]
        visibility.text = resources.getStringArray(R.array.visibilities)[if (deck.isHidden) 1 else 0]
        creator.text = deck.author

        editLink.setOnClickListener {
            findNavController().navigate(
                R.id.action_deckDetailFragment_to_newDeckFragment,
                Bundle().apply { putSerializable(DECK_DATA_KEY, deck as DeckData) })
        }
        addButton.setOnClickListener {
            findNavController().navigate(R.id.action_deckDetailFragment_to_newFlashcardFragment, Bundle().apply { putInt(DECK_ID_KEY, deck.id) })
        }
        playButton.setOnClickListener {
            val intent = Intent(requireActivity(), StudyActivity::class.java)
            intent.putExtra(DECK_ID_KEY, deckId)
            intent.putExtra(IS_OWNER_EXTRA, viewModel.isOwner)
            startActivity(intent)
        }
    }

    private fun initViewModel(id: Int) {
        viewModel.loadDetails(id).observe(viewLifecycleOwner, { deck ->
            adapter.setItems(deck.flashcards.map {
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
        findNavController().navigate(R.id.action_deckDetailFragment_to_newFlashcardFragment,
            Bundle().apply {
                putInt(DECK_ID_KEY, deckId)
                putInt(FLASHCARD_ID_KEY, id)
                putString(QUESTION_KEY, question)
                putString(ANSWER_KEY, answer)
            })
    }

    override fun onShowAnswerChanged() {
        adapter.notifyDataSetChanged()
    }

}