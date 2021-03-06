package com.ort.studyup.home.decks

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.observe
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import com.ort.studyup.R
import com.ort.studyup.common.DECK_ID_KEY
import com.ort.studyup.common.renderers.DeckItemRenderer
import com.ort.studyup.common.renderers.EmptyViewRenderer
import com.ort.studyup.common.renderers.SubtitleRenderer
import com.ort.studyup.common.ui.BaseFragment
import com.thinkup.easylist.RendererAdapter
import kotlinx.android.synthetic.main.fragment_decks.*

class DecksFragment : BaseFragment(), DeckItemRenderer.Callback {

    private val adapter = RendererAdapter()
    private val viewModel: DecksViewModel by injectViewModel(DecksViewModel::class)

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_decks, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()
        initViewModel()
    }

    private fun initUI() {
        fab.setOnClickListener {
            findNavController().navigate(R.id.action_decksFragment_to_newDeckFragment)
        }
        adapter.addRenderer(SubtitleRenderer())
        adapter.addRenderer(DeckItemRenderer(this))
        adapter.setEmptyItem(EmptyViewRenderer.Item(getString(R.string.no_decks)), EmptyViewRenderer())
        deckList.layoutManager = LinearLayoutManager(requireContext())
        deckList.adapter = adapter
    }

    private fun initViewModel() {
        viewModel.loadDecks().observe(viewLifecycleOwner) {
            adapter.setItems(it)
        }
    }

    override fun onDeckClicked(deckId: Int) {
        findNavController().navigate(R.id.action_decksFragment_to_deckDetailFragment, Bundle().apply { putInt(DECK_ID_KEY, deckId) })
    }

}