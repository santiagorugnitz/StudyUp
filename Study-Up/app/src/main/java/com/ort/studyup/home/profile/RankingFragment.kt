package com.ort.studyup.home.profile

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.observe
import androidx.recyclerview.widget.LinearLayoutManager
import com.ort.studyup.R
import com.ort.studyup.common.renderers.ResultItemRenderer
import com.ort.studyup.common.ui.BaseFragment
import com.thinkup.easylist.RendererAdapter
import kotlinx.android.synthetic.main.fragment_ranking.*

class RankingFragment : BaseFragment() {

    private val viewModel: RankingViewModel by injectViewModel(RankingViewModel::class)
    private val adapter = RendererAdapter()

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_ranking, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()
        initViewModel()
    }

    private fun initViewModel() {
        viewModel.loadRanking().observe(viewLifecycleOwner) {
            adapter.setItems(it)
        }
    }

    private fun initUI() {
        adapter.addRenderer(ResultItemRenderer())
        scoreList.layoutManager = LinearLayoutManager(requireContext())
        scoreList.adapter = adapter
    }

}