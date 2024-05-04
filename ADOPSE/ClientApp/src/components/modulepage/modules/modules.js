import React, { useEffect, useState } from "react";
import "./modules.scss";
import Module from "./module";
import Paginator from "./paginator";
import { useNavigate, useLocation } from "react-router-dom";

function Modules(props) {
  const navigate = useNavigate();
  const location = useLocation();
  const [activeIndex, setActiveIndex] = useState(
    parseInt(new URLSearchParams(location.search).get("page")) || 1
  );
  const [limit] = useState(12);
  const [offset, setOffset] = useState(0);
  const [modules, setModules] = useState([]);
  const [pages, setPages] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [failedToLoad, setFailedToLoad] = useState(false);

  const [count, setCount] = useState(null);
  const [infoToggle, setInfoToggle] = useState(false);
  const [advancedSearchToggle, setAdvancedSearchToggle] = useState(false);
  const [isLoadingEnrolled, setIsLoadingEnrolled] = useState(true);

  const [searchQuery, setSearchQuery] = useState("");
  const [titleSearchQuery, setTitleSearchQuery] = useState("");
  const [descriptionSearchQuery, setDescriptionSearchQuery] = useState("");
  const [moduleIds, setModuleIds] = useState();
  const [isEnrolled, setIsEnrolled] = useState({});
  const [trigger, setTrigger] = useState(false);

  const handleSearchChange = (event) => {
    setSearchQuery(event.target.value);
  };
  const handleKeyPress = (event) => {
    if (event.key === 'Enter') {
      setTrigger(!trigger);
    }
  }

  const triggerSearch = () => {
    setTrigger(!trigger);
  }

  useEffect(() => {
    if (pages === null) return;
    if (activeIndex > pages) {
      setActiveIndex(pages);
    }
  }, [pages, activeIndex]);


  useEffect(() => {

    async function fetchModules(retryCount = 0) {

      let query = "";
      let titleQuery = "";
      let descriptionQuery = "";
      if (trigger) {
        if (searchQuery.trim().length > 0) {
          query = "(" + searchQuery + ")"
        }
        if (titleSearchQuery.trim().length > 0) {
          let words = titleSearchQuery.split(" ");
          for (let i = 0; i < words.length; i++)
            titleQuery += `Name:${words[i]} AND `;

          titleQuery = titleQuery.slice(0, -5);
          console.log(titleQuery)
          if (query.length > 0)
            query += " AND " + titleQuery;
          else
            query += titleQuery;
        }
        if (descriptionSearchQuery.trim().length > 0) {
          if (titleQuery.length > 0 || query.trim().length > 0)
            query += " AND "
          let words = descriptionSearchQuery.split(" ");

          for (let i = 0; i < words.length; i++)
            descriptionQuery += `Description:${words[i]} AND `;

          descriptionQuery = descriptionQuery.slice(0, -5);
          query += descriptionQuery;
        }
      }
      console.log(query);
      const maxRetries = 3;
      try {
        const response = await Promise.race([
          fetch(
            `/api/module/filtered/${limit}/${offset}
              /?ModuleTypeId=${props.type}
              &DifficultyId=${props.difficulty}
              &price=${props.priceRange[0]},${props.priceRange[1]}
              &Rating=${props.stars[0]},${props.stars[1]}
              &SearchQuery=${query}`
          ),
          new Promise((_, reject) =>
            setTimeout(() => reject(new Error("Timeout")), 5000)
          ),
        ]);
        const data = await response.json();
        setCount(data.count);
        setModules(data.modules);
        setModuleIds(data.modules.map((module) => module.id));
        setPages(Math.ceil(data.count / limit));
        setIsLoading(false);
      } catch (error) {

        console.error(error);
        if (retryCount < maxRetries) {
          console.log(retryCount);
          retryCount++;
          console.log(`Retrying fetch... Attempt ${retryCount}`);
          fetchModules(retryCount);
        } else {
          console.error(`Failed to fetch modules after ${maxRetries} attempts`);
          setFailedToLoad(true);
        }
      }
    }

    setIsLoading(true);
    fetchModules(0);
  }, [limit, offset, props.stars, props.priceRange, props.type, props.difficulty, trigger]);

  useEffect(() => {
    let retryCount = 0;
    const maxRetries = 3;
    setIsLoadingEnrolled(true);

    async function fetchIsEnrolled() {
      try {
        const headers = {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
          "Content-Type": "application/json",
        };

        const requestBody = JSON.stringify({ moduleIds });

        const response = await Promise.race([
          fetch("/api/enrolled/getIsEnrolled", {
            method: "POST",
            headers,
            body: requestBody,
          }),
          new Promise((_, reject) =>
            setTimeout(() => reject(new Error("Timeout")), 5000)
          ),
        ]);
        const data = await response.json();

        if (data.authorized === true) {
          const enrolledStatuses = data.isEnrolled.reduce((obj, item) => {
            obj[item.moduleId] = item.isEnrolled;
            return obj;
          }, {});
          setIsEnrolled(enrolledStatuses);
        }
        setIsLoadingEnrolled(false);
      } catch (error) {
        console.error(error);
        if (retryCount < maxRetries) {
          retryCount++;
          console.log(`Retrying fetch... Attempt ${retryCount}`);
          fetchIsEnrolled();
        } else {
          console.error(
            `Failed to fetch enrollment statuses after ${maxRetries} attempts`
          );
        }
      }
    }

    if (moduleIds) {
      fetchIsEnrolled();
    }
  }, [moduleIds]);

  useEffect(() => {
    // if (pages === 0) return;
    console.log("Active Index: ", activeIndex);
    const newOffset = (activeIndex - 1) * limit;
    if (newOffset < 0) {
      setOffset(0);
    } else {
      setOffset(newOffset);
    }

    navigate(`?page=${activeIndex}`, { replace: true });
  }, [activeIndex, limit, navigate, pages]);

  return (
    <div className="modules">
      <div className="search-bar">
        <div className="default-search-bar">
          <div className="lucene-info">
            <button onClick={() => setInfoToggle(!infoToggle)}>
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-info-circle" viewBox="0 0 16 16">
                <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14m0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16" />
                <path d="m8.93 6.588-2.29.287-.082.38.45.083c.294.07.352.176.288.469l-.738 3.468c-.194.897.105 1.319.808 1.319.545 0 1.178-.252 1.465-.598l.088-.416c-.2.176-.492.246-.686.246-.275 0-.375-.193-.304-.533zM9 4.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0" />
              </svg>
            </button>
            {infoToggle && <div className="lucene-info-content popup">
              <div className='popup-content'>
                <div className="popup-exit">
                  <button onClick={() => setInfoToggle(!infoToggle)}>
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x-circle" viewBox="0 0 16 16">
                      <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14m0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16" />
                      <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708" />
                    </svg>
                  </button>
                </div>
                <h2>Lucene Wildcard Searches</h2>
                <p>Lucene supports single and multiple character wildcard searches within single terms (not within phrase queries).</p>
                <p>To perform a single character wildcard search, use the <code>?</code> symbol.</p>
                <p>To perform a multiple character wildcard search, use the <code>*</code> symbol.</p>
                <p>The single character wildcard search looks for terms that match with the single character replaced. For example, to search for "text" or "test", you can use the search:</p>
                <p><code>te?t</code></p>
                <p>Multiple character wildcard searches look for 0 or more characters. For example, to search for "test", "tests", or "tester", you can use the search:</p>
                <p><code>test*</code></p>
                <p>You can also use wildcard searches in the middle of a term.</p>
                <p>For example, to search for any term starting with "te" and ending with "t", you can use:</p>
                <p><code>te*t</code></p>

                <h2>Lucene Boolean Operators</h2>

                <p>Boolean operators allow terms to be combined through logic operators. Lucene supports <code>AND</code>, <code>+</code>, <code>OR</code>, <code>NOT</code>, and <code>-</code> as Boolean operators (Note: Boolean operators must be ALL CAPS).</p>
                <p>The <code>OR</code> operator links two terms and finds a matching document if either of the terms exists in a document. This is equivalent to a union using sets. The symbol <code>||</code> can be used in place of the word <code>OR</code>.</p>
                <p>To search for documents that contain either "jakarta apache" or just "jakarta", use the query:</p>
                <p><code>"jakarta apache" OR jakarta</code></p>
                <p>The <code>AND</code> operator matches documents where both terms exist anywhere in the text of a single document. This is equivalent to an intersection using sets. The symbol <code>&&</code> can be used in place of the word <code>AND</code>.</p>
                <p>To search for documents that contain "jakarta apache" and "Apache Lucene", use the query:</p>
                <p><code>"jakarta apache" AND "Apache Lucene"</code></p>
                <p>The <code>+</code> or required operator requires that the term after the <code>+</code> symbol exists somewhere in a field of a single document.</p>
                <p>To search for documents that must contain "jakarta" and may contain "lucene", use the query:</p>
                <p><code>+jakarta lucene</code></p>
                <p>The <code>NOT</code> operator excludes documents that contain the term after <code>NOT</code>. This is equivalent to a difference using sets. The symbol <code>!</code> can be used in place of the word <code>NOT</code>.</p>
                <p>To search for documents that contain "jakarta apache" but not "Apache Lucene", use the query:</p>
                <p><code>"jakarta apache" NOT "Apache Lucene"</code></p>
                <p>Note: The <code>NOT</code> operator cannot be used with just one term. For example, the following search will return no results:</p>
                <p><code>NOT "jakarta apache"</code></p>
                <button onClick={() => setInfoToggle(!infoToggle)}>Close</button>
              </div>
            </div>}
          </div>
          <input
            className="search-query-input"
            type="text"
            name="searchQueryInput"
            placeholder="Search"
            value={searchQuery}
            onChange={handleSearchChange}
            onKeyDown={handleKeyPress}
          />
          <button
            className="search-query-submit"
            type="submit"
            name="searchQuerySubmit"
            onClick={triggerSearch}
          >Search
            <svg viewBox="0 0 24 24">
              <path
                fill="#666666"
                d="M9.5,3A6.5,6.5 0 0,1 16,9.5C16,11.11 15.41,12.59 14.44,13.73L14.71,14H15.5L20.5,19L19,20.5L14,15.5V14.71L13.73,14.44C12.59,15.41 11.11,16 9.5,16A6.5,6.5 0 0,1 3,9.5A6.5,6.5 0 0,1 9.5,3M9.5,5C7,5 5,7 5,9.5C5,12 7,14 9.5,14C12,14 14,12 14,9.5C14,7 12,5 9.5,5Z"
              />
            </svg>
          </button>
        </div>
        <div className="search-options">
          <button onClick={() => { setAdvancedSearchToggle(!advancedSearchToggle); setDescriptionSearchQuery(""); setTitleSearchQuery(""); }}>Advanced Search</button>
        </div>
        {(advancedSearchToggle) && <div className="extra-search-bar">
          <label>
            Search in title
          </label>
          <input
            className="search-query-input"
            type="text"
            name="titleSearchQueryInput"
            placeholder="Search In title"
            value={titleSearchQuery}
            onChange={(e) => setTitleSearchQuery(e.target.value)}
            onKeyDown={handleKeyPress} />
        </div>}
        {(advancedSearchToggle) && <div className="extra-search-bar">
          <label>
            Search in description
          </label>
          <input
            className="search-query-input"
            type="text"
            name="descriptionSearchQueryInput"
            placeholder="Search In Description"
            value={descriptionSearchQuery}
            onChange={(e) => setDescriptionSearchQuery(e.target.value)}
            onKeyDown={handleKeyPress}
          />
        </div>}

      </div>
      {modules && <div className="result-count">
        <span>Total Modules Found: {count}</span>
        {modules.length > 0 && <span>Showing {offset} - {offset + modules.length}</span>}
      </div>}
      <br />
      {isLoading ? ( // check if loading is true
        failedToLoad ? (
          <div>Failed to load modules. Please try again later.</div>
        ) : (
          <div>Loading...</div>
        )
      ) : (
        <>
          {pages > 0 ? (
            <div className="modules-main">
              {modules.map((module, index) => (
                <Module
                  key={module.id}
                  id={module.id}
                  index={index}
                  school={module.name}
                  subject={module.name}
                  subject_type={module.moduleTypeName}
                  description={module.description}
                  difficulty={module.difficultyName}
                  rating={module.rating}
                  enrolled={module.price}
                  price={module.price}
                  isEnrolled={isEnrolled[module.id]}
                  isLoadingEnrolled={isLoadingEnrolled}
                  searchQuery={searchQuery}
                />
              ))}
            </div>
          ) : (
            <div>No modules found for the selected Filters.</div>
          )}
          {pages > 0 && (
            <Paginator
              pageCount={pages}
              setActiveIndex={setActiveIndex}
              activeIndex={activeIndex}
            />
          )}
        </>
      )}
    </div>
  );
}
export default Modules;
