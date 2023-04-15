import React, { useState } from "react";
import { Paginator } from "./paginator";
import { Items } from "./items";
import Main from "./main";

const items = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14];

export function Home() {
  const [currentItems, setCurrentItems] = useState(items);
  return (
    <div>
      <Main />
      <Items currentItems={currentItems} />
      <Paginator
        items={items}
        itemsPerPage={4}
        setCurrentItems={setCurrentItems}
      />
    </div>
  );
}
