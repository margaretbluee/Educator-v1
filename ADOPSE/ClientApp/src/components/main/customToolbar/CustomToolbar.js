import React, {useState} from "react";
import "./toolbarStyle.scss";

const CustomToolbar = ({ label, onNavigate, onView, toggleModuleList }) => {
  
  const [selectedView, setSelectedView] = useState('month');

  const handleViewChange = (view) => {
    onView(view)
    setSelectedView(view)
  }

  return (
    <div className="calendar-toolbar">
      <div className='left-side-buttons'>
        <button className="left-button" onClick={() => onNavigate('PREV')}>Prev</button>
        <button onClick={() => onNavigate('TODAY')}>Today</button>
        <button className="right-button" onClick={() => onNavigate('NEXT')}>Next</button>        
      </div>
      <span className="date-label">{label}</span>
      <div className='right-side-buttons'>
        <button
         className='left-button'
         style={{ backgroundColor: selectedView === 'month' ? '#74a4ce' : '#f0f0f0 ',
                  color: selectedView === 'month' ? '#fff' : '#000 '
          }}
         onClick={() => handleViewChange('month')}
         >
          Month
        </button>
        <button
         style={{ backgroundColor: selectedView === 'week' ? '#74a4ce' : '#f0f0f0 ',
                  color: selectedView === 'week' ? '#fff' : '#000 '}} 
         onClick={() => handleViewChange('week')}
         >
          Week
        </button>
        <button 
         style={{ backgroundColor: selectedView === 'day' ? '#74a4ce' : '#f0f0f0 ',
                  color: selectedView === 'day' ? '#fff' : '#000 '}}
         onClick={() => handleViewChange('day')}
         >
          Day
        </button>
        <button 
         style={{ backgroundColor: selectedView === 'agenda' ? '#74a4ce' : '#f0f0f0 ',
                  color: selectedView === 'agenda' ? '#fff' : '#000 '}}
         onClick={() => handleViewChange('agenda')}
         >
          Agenda
        </button>
        <button
          className="right-button"
          onClick={toggleModuleList}
        >
          List
        </button> 
      </div>
    </div>
  );
};

export default CustomToolbar;
