import React from 'react';
import { useIngredients } from '../api';

function IngredientList() {
	const { data: ingredientResponse, isLoading } = useIngredients({});
	const ingredientData = ingredientResponse?.data;
	// const ingredientPagination = ingredientResponse?.pagination;

	if(isLoading) 
		return <div>Loading...</div>

	return (
		<>
			{
				ingredientData && ingredientData.length > 0 ? (
					ingredientData?.map((ingredient) => {
						return <div key={ingredient.id}>{ingredient.id}</div>;
					})
				) : (
					<div>No Ingredients Found</div>
				)}
		</>
	)
}

export { IngredientList };
