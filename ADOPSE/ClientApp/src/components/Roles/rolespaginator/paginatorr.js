import React, { useState } from "react";
import ReactPaginate from "react-paginate";
import "./paginatorr.scss";

function Paginatorr(props) {
  const [forcePage] = useState(
    Math.min(props.activeIndex - 1, props.pageCount - 1)
  );

  const handlePageClick = (event) => {
    const selectedPage = event.selected + 1;
    props.setActiveIndex(selectedPage);
  };

  return (
    <div className="paginator">
      <nav aria-label="Page navigation comments" className="mt-4">
        <ReactPaginate
          previousLabel="previous"
          nextLabel="next"
          breakLabel="..."
          breakClassName="page-item"
          breakLinkClassName="page-link"
          pageCount={props.pageCount}
          pageRangeDisplayed={4}
          marginPagesDisplayed={2}
          onPageChange={handlePageClick}
          containerClassName="pagination justify-content-center"
          pageClassName="page-class"
          pageLinkClassName="page-link"
          previousClassName="page-item"
          previousLinkClassName="page-link"
          nextClassName="page-item"
          nextLinkClassName="page-link"
          activeClassName="active"
          eslint-disable-next-line
          no-unused-vars
          forcePage={forcePage}
          hrefAllControls
        />
      </nav>
    </div>
  );
}
export default Paginatorr;