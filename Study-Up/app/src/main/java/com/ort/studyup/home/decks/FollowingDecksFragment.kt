package com.ort.studyup.home.decks

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import androidx.lifecycle.observe
import androidx.recyclerview.widget.LinearLayoutManager
import com.ort.studyup.R
import com.ort.studyup.common.renderers.SubtitleRenderer
import com.ort.studyup.common.ui.BaseFragment
import com.thinkup.easylist.RendererAdapter
import kotlinx.android.synthetic.main.fragment_following_decks.*
import kotlinx.android.synthetic.main.item_spinner.view.*

class FollowingDecksFragment : BaseFragment() {

    private val adapter = RendererAdapter()
    private val viewModel: FollowingDecksViewModel by injectViewModel(FollowingDecksViewModel::class)

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_following_decks, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()
        initViewModel()
    }

    private fun initUI() {
        adapter.addRenderer(SubtitleRenderer())
        deckList.layoutManager = LinearLayoutManager(requireContext())
        deckList.adapter = adapter

        authorSpinner.spinner.onItemSelectedListener = object : AdapterView.OnItemSelectedListener {
            override fun onItemSelected(parent: AdapterView<*>?, selectedItem: View?, position: Int, id: Long) {
                viewModel.filterDecks(if (position == 0) null else selectedItem?.toString()).observe(viewLifecycleOwner) {
                    adapter.setItems(it)
                }
            }

            override fun onNothingSelected(p0: AdapterView<*>?) {
            }
        }
    }

    private fun initViewModel() {
        viewModel.loadDecks().observe(viewLifecycleOwner) {
            initSpinner(authorSpinner, viewModel.authors, getString(R.string.author))
            adapter.setItems(it)
        }
    }

}