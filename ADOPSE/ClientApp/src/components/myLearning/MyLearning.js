import React from 'react';
import {hasJWT} from "../authentication/authentication";
function MyLearning() {
    return (
        <div>
            {
                hasJWT() ? 
                    <div>
                        You are auth
                    </div> :
                    <div>
                        No auth
                    </div>
            }
        </div>
    );
}

export default MyLearning;