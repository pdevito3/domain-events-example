import React from 'react';
import { useRecipes } from '../api';

function RecipeList() {
	const { data: recipeResponse, isLoading } = useRecipes({});
	const recipeData = recipeResponse?.data;
	// const recipePagination = recipeResponse?.pagination;

	if(isLoading) 
		return <div>Loading...</div>

	return (
		<>
			{
				recipeData && recipeData.length > 0 ? (
					recipeData?.map((recipe) => {
						return <div key={recipe.id}>{recipe.id}</div>;
					})
				) : (
					<div>No Recipes Found</div>
				)}
		</>
	)
}

export { RecipeList };
