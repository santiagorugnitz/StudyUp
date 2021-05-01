package com.ort.studyup.home.search

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.inputmethod.EditorInfo
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.LinearLayoutManager
import com.ort.studyup.R
import com.ort.studyup.common.QR_EXTRA
import com.ort.studyup.common.SCAN_ACTIVITY_REQUEST_CODE
import com.ort.studyup.common.renderers.GroupSearchResultRenderer
import com.ort.studyup.common.renderers.UserSearchResultRenderer
import com.ort.studyup.common.ui.BaseFragment
import com.thinkup.easylist.RendererAdapter
import kotlinx.android.synthetic.main.fragment_search.*

class SearchFragment : BaseFragment(), UserSearchResultRenderer.Callback, GroupSearchResultRenderer.Callback {

    private val viewModel: SearchViewModel by injectViewModel(SearchViewModel::class)
    private val adapter = RendererAdapter()

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_search, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        prepareList()
        initUI()
        initRefresh()
    }

    private fun prepareList() {
        adapter.addRenderer(UserSearchResultRenderer(this))
        adapter.addRenderer(GroupSearchResultRenderer(this))
        searchResultsList.layoutManager = LinearLayoutManager(requireContext())
        searchResultsList.adapter = adapter
    }

    private fun initRefresh() {
        swipeRefresh.setOnRefreshListener {
            swipeRefresh.isRefreshing = false
            onSearch(searchInput.text.toString())
        }
    }

    private fun initUI() {
        initSearchBar()
        onSearch("")
        qrScan.setOnClickListener {
            val intent = Intent(context, ScanActivity::class.java)
            startActivityForResult(intent, SCAN_ACTIVITY_REQUEST_CODE)
        }

        searchInput.setOnEditorActionListener { textView, id, _ ->
            if (id == EditorInfo.IME_ACTION_SEARCH) {
                onSearch(textView.text.toString())
                true
            }
            false
        }

    }

    private fun initSearchBar() {
        searchBar.setOnNavigationItemSelectedListener {
            when (it.itemId) {
                R.id.searchGroups -> {
                    viewModel.searchUsers = false
                    onSearch(searchInput.text.toString())
                }
                R.id.searchUsers -> {
                    viewModel.searchUsers = true
                    onSearch(searchInput.text.toString())
                }
            }
            true
        }
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (requestCode == SCAN_ACTIVITY_REQUEST_CODE) {
            data?.getStringExtra(QR_EXTRA)?.let {
                searchInput.setText(it)
                onSearch(it)
            }

        }
    }

    private fun onSearch(query: String) {
        hideKeyboard()
        viewModel.search(query).observe(viewLifecycleOwner, Observer {
            adapter.setItems(it)
        })
    }

    override fun onFollowChange(position: Int) {
        viewModel.onFollowChange(position).observe(viewLifecycleOwner, Observer {
            if (it) {
                adapter.notifyDataSetChanged()
            }
        })
    }

    override fun onSubChange(position: Int) {
        viewModel.onSubChange(position).observe(viewLifecycleOwner, Observer {
            if (it) {
                adapter.notifyDataSetChanged()
            }
        })
    }

}